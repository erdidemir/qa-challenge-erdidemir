using AutoMapper;
using CodingChallenge.Common.Constants;
using CodingChallenge.Common.Enums;
using CodingChallenge.Data;
using CodingChallenge.Data.Abstraction;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Factories.Abstractions;
using CodingChallenge.Service.MappingProfiles;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CodingChallenge.Service.UnitTests
{
    [TestCaseOrderer("CodingChallenge.Service.UnitTests.PriorityOrderer", "CodingChallenge.Service.UnitTests")]
    public class TransactionServiceUnitTests : IClassFixture<InMemoryCodingChallengeDbContext>
    {
        private readonly TransactionService _transactionServiceUnderTest;
        private readonly IMapper _mapper;
        private readonly Mock<ITransactionDataModelFactory> _mockITransactionDataModelFactory = new();

        private readonly ICodingChallengeDbContext _codingChallengeDbContext;

        public TransactionServiceUnitTests(InMemoryCodingChallengeDbContext inMemoryCodingChallengeDbContext)
        {
            _codingChallengeDbContext = inMemoryCodingChallengeDbContext.CodingChallengeDbContext;

            var transactionMappingProfile = new TransactionMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(transactionMappingProfile));
            _mapper = new Mapper(configuration);

            _transactionServiceUnderTest = new TransactionService(
                _codingChallengeDbContext,
                _mapper,
                _mockITransactionDataModelFactory.Object);
        }

        [Fact, TestPriority(1)]
        public async Task CallingGetTransactionsMethod_ShouldReturnEmptyCollection_WhenNoTransactionDataAvailable()
        {
            // Arrange
            const int pageNuber = ApplicationConstants.TransactionDefaultPageNumber;
            const int pageSize = ApplicationConstants.TransactionDefaultPageSize;
            CancellationTokenSource tokenSource = new();

            // Act
            IEnumerable<TransactionDto> transactionDtos = await _transactionServiceUnderTest.GetTransactions(
                pageNuber,
                pageSize,
                tokenSource.Token);

            // Assert
            transactionDtos.Should().BeEmpty();
        }

        [Fact, TestPriority(2)]
        public async Task CallingGetTransactionsMethod_ShouldReturnMappedCollection_WhenTransactionDataIsAvailable()
        {
            // Arrange
            const int pageNuber = 2;
            const int pageSize = 3;
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionDto> transactionDtos = await _transactionServiceUnderTest.GetTransactions(
                pageNuber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                transactionDtos.Should().NotBeEmpty();
                transactionDtos.Should().HaveCount(3);
            }
        }

        [Fact, TestPriority(3)]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnEmptyCollection_WhenNoHighVolumeTransactionDataAvailable()
        {
            // Arrange
            const decimal thresholdamount = decimal.MaxValue;
            const int pageNuber = ApplicationConstants.TransactionDefaultPageNumber;
            const int pageSize = ApplicationConstants.TransactionDefaultPageSize;
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionDto> transactionDtos = await _transactionServiceUnderTest.GetHighVolumeTransactions(
                thresholdamount,
                pageNuber,
                pageSize,
                tokenSource.Token);

            // Assert
            transactionDtos.Should().BeEmpty();
        }

        [Fact, TestPriority(4)]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnMappedCollection_WhenHighVolumeTransactionDataIsAvailable()
        {
            // Arrange
            const decimal thresholdamount = 1000;
            const int pageNuber = ApplicationConstants.TransactionDefaultPageNumber;
            const int pageSize = ApplicationConstants.TransactionDefaultPageSize;
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionDto> transactionDtos = await _transactionServiceUnderTest.GetHighVolumeTransactions(
                thresholdamount,
                pageNuber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                transactionDtos.Should().NotBeEmpty();
                transactionDtos.Should().HaveCountGreaterThanOrEqualTo(8);
            }
        }

        [Fact, TestPriority(5)]
        public async Task CallingGetTransactionByIdMethod_ShouldReturnNumm_WhenNoTransactionIdMatchingDataAvailable()
        {
            // Arrange
            const int transactionId = int.MaxValue;
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            TransactionDto? transactionDto = await _transactionServiceUnderTest.GetTransactionById(
                transactionId,
                tokenSource.Token);

            // Assert
            transactionDto.Should().BeNull();
        }

        [Fact, TestPriority(6)]
        public async Task CallingGetTransactionByIdMethod_ShouldReturnMappedDto_WhenMatchingTransactionDataIsAvailable()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            TransactionDataModel transactionDataModel = _codingChallengeDbContext.Transactions.Last();
            int transactionId = transactionDataModel.Id;
           
            // Act
            TransactionDto? transactionDto = await _transactionServiceUnderTest.GetTransactionById(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                transactionDto.Should().NotBeNull();
                transactionDto.TransactionId.Should().Be(transactionId);
                transactionDto.TransactionAmount.Should().Be(transactionDataModel.Amount);
                transactionDto.TransactionType.Should().Be(transactionDataModel.TransactionType);
                transactionDto.TransactionCreatedAt.Should().Be(transactionDataModel.CreatedAt);
                transactionDto.UserId.Should().Be(transactionDataModel.UserId);
            }
        }

        [Theory, TestPriority(7)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task CallingAddTransactionMethod_ShouldTransactionIdOfNewelyAddedTransaction(
            TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto addTransactionDto = new()
            {
                UserId = "TestUser1",
                TransactionAmount = 1000,
                TransactionType = transactionType
            };
            CancellationTokenSource tokenSource = new();
            TransactionDataModel newTransactionDataModel = new()
            {
                UserId = addTransactionDto.UserId,
                TransactionType = addTransactionDto.TransactionType,
                Amount = addTransactionDto.TransactionAmount,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await SeedDataForTests();

            // Setup
            _mockITransactionDataModelFactory.Setup(x => x.CreateTransactionDataModel(
                addTransactionDto)).Returns(newTransactionDataModel);

            // Act
            int transactionId = await _transactionServiceUnderTest.AddTransaction(
                addTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                transactionId.Should().BeGreaterThan(0);
                TransactionDataModel? transactionDataModel = await _codingChallengeDbContext.Transactions.SingleOrDefaultAsync(x => x.Id == transactionId);
                transactionDataModel.Should().NotBeNull();

                // Verify
                _mockITransactionDataModelFactory.Verify(x => x.CreateTransactionDataModel(
                    addTransactionDto), Times.Once());
            }
        }

        [Theory, TestPriority(8)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task CallingUpdateTransactionMethod_ShouldNotPerformUpdateAndReturnFalse_WhenTransactionIdMatchingRecordNotExists(
            TransactionTypes transactionType)
        {
            // Arrange
            const int transactionId = int.MaxValue;
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUser1",
                TransactionAmount = 1000,
                TransactionType = transactionType
            };
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            bool isSuccess = await _transactionServiceUnderTest.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                isSuccess.Should().BeFalse();

                // Verify
                _mockITransactionDataModelFactory.Verify(x => x.UpdateTransactionDataModel(
                    It.IsAny<TransactionDataModel>(),
                    updateTransactionDto),
                    Times.Never());
            }
        }

        [Theory, TestPriority(9)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task CallingUpdateTransactionMethod_ShouldPerformUpdateAndReturnTrue_WhenTransactionIdMatchingRecordIsExists(
           TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUser1",
                TransactionAmount = 1000,
                TransactionType = transactionType == TransactionTypes.Debit ? TransactionTypes.Credit : TransactionTypes.Debit,
            };
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            TransactionDataModel transactionDataModel = await _codingChallengeDbContext
                .Transactions
                .Where(x => x.TransactionType == transactionType).LastAsync();
            int transactionId = transactionDataModel.Id;

            TransactionDataModel updatedTransactionDataModel = new()
            {
                Id = transactionDataModel.Id,
                CreatedAt = transactionDataModel.CreatedAt,
                UpdatedAt = DateTime.UtcNow,
                UserId = updateTransactionDto.UserId,
                Amount = updateTransactionDto.TransactionAmount,
                TransactionType = updateTransactionDto.TransactionType,
            };

            // Setup
            _mockITransactionDataModelFactory.Setup(x => x.UpdateTransactionDataModel(
                transactionDataModel,
                updateTransactionDto))
                .Returns(updatedTransactionDataModel);

            // Act
            bool isSuccess = await _transactionServiceUnderTest.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                isSuccess.Should().BeTrue();

                // Verify
                _mockITransactionDataModelFactory.Verify(x => x.UpdateTransactionDataModel(
                    transactionDataModel,
                    updateTransactionDto),
                    Times.Once());
            }
        }

        [Fact, TestPriority(10)]
        public async Task CallingDeleteTransactionMethod_ShouldNotPerformDeleteAndReturnFalse_WhenTransactionIdMatchingRecordNotExists()
        {
            // Arrange
            const int transactionId = int.MaxValue;
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            bool isSuccess = await _transactionServiceUnderTest.DeleteTransaction(
                transactionId,
                tokenSource.Token);

            // Assert
            isSuccess.Should().BeFalse();
        }

        [Fact, TestPriority(11)]
        public async Task CallingUpdateTransactionMethod_ShouldPerformDeleteAndReturnTrue_WhenTransactionIdMatchingRecordIsExists()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            TransactionDataModel transactionDataModel = await _codingChallengeDbContext
                .Transactions.LastAsync();
            int transactionId = transactionDataModel.Id;

            // Act
            bool isSuccess = await _transactionServiceUnderTest.DeleteTransaction(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                isSuccess.Should().BeTrue();

                TransactionDataModel? deletedTransactionDataModel = await _codingChallengeDbContext
                .Transactions
                .SingleOrDefaultAsync(x => x.Id == transactionId);

                deletedTransactionDataModel.Should().BeNull();
            }
        }

        private async Task SeedDataForTests()
        {
            if (!_codingChallengeDbContext.Transactions.Any())
            {
                IList<TransactionDataModel> transactionDataModels = TransactionDataModelHelper.CreateTransactionDataModels(5);
                await _codingChallengeDbContext.Transactions.AddRangeAsync(transactionDataModels);
                await _codingChallengeDbContext.SaveChangesAsync();
            }
        }
    }
}
