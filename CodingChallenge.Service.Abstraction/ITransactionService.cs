using CodingChallenge.Common.Constants;
using CodingChallenge.Dtos;

namespace CodingChallenge.Service.Abstraction
{
    /// <summary>
    /// The ITransactionService interface which define transaction operation.
    /// </summary>
    public interface ITransactionService
    {
        /// <summary>
        /// Get the transactions.
        /// </summary>
        /// <param name="pageNumber">The page number value.</param>
        /// <param name="pageSize">The page size value.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A list of transactions <seealso cref="TransactionDto"/>.</returns>
        Task<IEnumerable<TransactionDto>> GetTransactions(
            int pageNuber = ApplicationConstants.TransactionDefaultPageNumber,
            int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the high volume transactions.
        /// </summary>
        /// <param name="thresholdAmount">The threshold amount value.</param>
        /// <param name="pageNumber">The page number value.</param>
        /// <param name="pageSize">The page size value.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The list of high volumne transactions <seealso cref="TransactionDto"/>.</returns>
        Task<IEnumerable<TransactionDto>> GetHighVolumeTransactions(
            decimal thresholdamount,
            int pageNuber = ApplicationConstants.TransactionDefaultPageNumber,
            int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get a transaction by ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The requested transaction <seealso cref="TransactionDto"/>.</returns>
        Task<TransactionDto?> GetTransactionById(
            int transactionId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a Transaction.
        /// </summary>
        /// <param name="addTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The Id value of added transaction.</returns>
        Task<int> AddTransaction(
            AddOrUpdateTransactionDto addTransactionDto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Update the existing Transaction By Id.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="updateTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        Task<bool> UpdateTransaction(
            int transactionId,
            AddOrUpdateTransactionDto updateTransactionDto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete the existing Transaction By Id.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        Task<bool> DeleteTransaction(
            int transactionId,
            CancellationToken cancellationToken = default);
    }
}
