using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;

namespace CodingChallenge.Service.Factories.Abstractions
{
    /// <summary>
    /// The transaction data model factory interface which define factory methods.
    /// </summary>
    public interface ITransactionDataModelFactory
    {
        /// <summary>
        /// The create transaction data model.
        /// </summary>
        /// <param name="addTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> dto.</param>
        /// <returns>The created transaction <seealso cref="TransactionDataModel"/> data model object.</returns>
        TransactionDataModel CreateTransactionDataModel(
            AddOrUpdateTransactionDto addTransactionDto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDataModel">The transaction <seealso cref="TransactionDataModel"/> data model.</param>
        /// <param name="updateTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> dto.</param>
        /// <returns>The updated transaction <seealso cref="TransactionDataModel"/> data model object.</returns>
        TransactionDataModel UpdateTransactionDataModel(
            TransactionDataModel transactionDataModel,
            AddOrUpdateTransactionDto updateTransactionDto);
    }
}
