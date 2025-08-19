using CodingChallenge.Common.Enums;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Factories;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;

namespace CodingChallenge.Service.UnitTests.Factories
{
    public class TransactionDataModelFactoryUnitTests
    {
        private readonly TransactionDataModelFactory _transactionDataModelFactoryUnderTest;
        private readonly Mock<TimeProvider> _mockTimeProvider = new();

        public TransactionDataModelFactoryUnitTests()
        {
            _transactionDataModelFactoryUnderTest = new TransactionDataModelFactory(
                _mockTimeProvider.Object);
        }

        [Theory]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public void CallingCreateTransactionDataModelMethod_ShouldReturnMappedModel(
            TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto addTransactionDto = new()
            {
                UserId = "TestUser1",
                TransactionAmount = 1500,
                TransactionType = transactionType,
            };
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;

            // Setup
            _mockTimeProvider.Setup(x => x.GetUtcNow()).Returns(dateTimeOffset);

            // Act
            TransactionDataModel transactionDataModel = _transactionDataModelFactoryUnderTest.CreateTransactionDataModel(
                addTransactionDto);

            // Assert
            using (new AssertionScope())
            {
                transactionDataModel.Should().NotBeNull();
                transactionDataModel.UserId.Should().Be(addTransactionDto.UserId);
                transactionDataModel.TransactionType.Should().Be(addTransactionDto.TransactionType);
                transactionDataModel.Amount.Should().Be(addTransactionDto.TransactionAmount);
                transactionDataModel.CreatedAt.Should().Be(dateTimeOffset.DateTime);
                transactionDataModel.UpdatedAt.Should().Be(dateTimeOffset.DateTime);

                // Verify
                _mockTimeProvider.Verify(x => x.GetUtcNow(), Times.Once());
            }
        }

        [Theory]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public void CallingUpdateTransactionDataModelMethod_ShouldReturnMappedModel(
           TransactionTypes transactionType)
        {
            // Arrange
            DateTimeOffset dateTimeOffset = DateTimeOffset.UtcNow;
            TransactionDataModel existingTransactionDataModel = new()
            {
                Id = 101,
                UserId = "TestUser1Old",
                TransactionType = transactionType == TransactionTypes.Debit ? TransactionTypes.Credit : TransactionTypes.Debit,
                Amount = 1200,
                CreatedAt = dateTimeOffset.AddDays(-1).DateTime,
                UpdatedAt = dateTimeOffset.AddDays(-1).DateTime,
            };
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUser1",
                TransactionAmount = 1500,
                TransactionType = transactionType,
            };
            

            // Setup
            _mockTimeProvider.Setup(x => x.GetUtcNow()).Returns(dateTimeOffset);

            // Act
            TransactionDataModel transactionDataModel = _transactionDataModelFactoryUnderTest.UpdateTransactionDataModel(
                existingTransactionDataModel,
                updateTransactionDto);

            // Assert
            using (new AssertionScope())
            {
                transactionDataModel.Should().NotBeNull();
                transactionDataModel.UserId.Should().Be(updateTransactionDto.UserId);
                transactionDataModel.TransactionType.Should().Be(updateTransactionDto.TransactionType);
                transactionDataModel.Amount.Should().Be(updateTransactionDto.TransactionAmount);
                transactionDataModel.CreatedAt.Should().Be(existingTransactionDataModel.CreatedAt);
                transactionDataModel.UpdatedAt.Should().Be(dateTimeOffset.DateTime);

                // Verify
                _mockTimeProvider.Verify(x => x.GetUtcNow(), Times.Once());
            }
        }
    }
}
