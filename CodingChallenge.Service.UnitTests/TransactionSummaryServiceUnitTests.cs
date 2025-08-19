using CodingChallenge.Common.Constants;
using CodingChallenge.Data;
using CodingChallenge.Data.Abstraction;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using FluentAssertions;
using Moq;

namespace CodingChallenge.Service.UnitTests
{
    [TestCaseOrderer("CodingChallenge.Service.UnitTests.PriorityOrderer", "CodingChallenge.Service.UnitTests")]
    public class TransactionSummaryServiceUnitTests : IClassFixture<InMemoryCodingChallengeDbContext>
    {
        private readonly TransactionSummaryService _transactionSummaryServiceUnderTest;

        private readonly ICodingChallengeDbContext _codingChallengeDbContext;

        public TransactionSummaryServiceUnitTests(InMemoryCodingChallengeDbContext inMemoryCodingChallengeDbContext)
        {
            _codingChallengeDbContext = inMemoryCodingChallengeDbContext.CodingChallengeDbContext;

            _transactionSummaryServiceUnderTest = new TransactionSummaryService(
                _codingChallengeDbContext);
        }

        [Fact, TestPriority(51)]
        public async Task CallingGetTransactionsByTransactionTypeMethod_ShouldReturnExtendData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByTransactionType(
                tokenSource.Token);

            // Assert
            transactionByTransactionTypeDtos.Should().NotBeEmpty();
        }

        [Fact, TestPriority(52)]
        public async Task CallingGetTransactionsByUserMethod_ShouldReturnExtendData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            await SeedDataForTests();

            // Act
            IEnumerable<TransactionByUserDto> transactionByUserDtos = await _transactionSummaryServiceUnderTest.GetTransactionsByUser(
                tokenSource.Token);

            // Assert
            transactionByUserDtos.Should().NotBeEmpty();
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
