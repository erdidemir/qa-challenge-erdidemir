using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Factories.Abstractions;

namespace CodingChallenge.Service.Factories
{
    /// <summary>
    /// The imlementation for <seealso cref="ITransactionDataModelFactory"/> which define factory methods.
    /// </summary>
    /// <param name="timeProvider">The instance of <seealso cref="TimeProvider"/>.</param>
    public class TransactionDataModelFactory(TimeProvider timeProvider) : ITransactionDataModelFactory
    {
        /// <inheritdoc />
        public TransactionDataModel CreateTransactionDataModel(
            AddOrUpdateTransactionDto addTransactionDto)
        {
            DateTime currentDateTime = timeProvider.GetUtcNow().DateTime;
            return new TransactionDataModel()
            {
                UserId = addTransactionDto.UserId,
                Amount = addTransactionDto.TransactionAmount,
                TransactionType = addTransactionDto.TransactionType,
                CreatedAt = currentDateTime,
                UpdatedAt = currentDateTime,
            };
        }

        /// <inheritdoc />
        public TransactionDataModel UpdateTransactionDataModel(
            TransactionDataModel updateTransactionDataModel,
            AddOrUpdateTransactionDto transactionDto)
        {
            updateTransactionDataModel.TransactionType = transactionDto.TransactionType;
            updateTransactionDataModel.Amount = transactionDto.TransactionAmount;
            updateTransactionDataModel.UserId = transactionDto.UserId;
            updateTransactionDataModel.UpdatedAt = timeProvider.GetUtcNow().DateTime;

            return updateTransactionDataModel;
        }
    }
}
