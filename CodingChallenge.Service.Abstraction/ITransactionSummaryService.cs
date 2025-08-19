using CodingChallenge.Common.Constants;
using CodingChallenge.Dtos;

namespace CodingChallenge.Service.Abstraction
{
    /// <summary>
    /// The ITransactionSummaryService interface which define transaction summary operation.
    /// </summary>
    public interface ITransactionSummaryService
    {
        /// <summary>
        /// Get the transactions by user.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A list of transactions <seealso cref="TransactionByUserDto"/>.</returns>
        Task<IEnumerable<TransactionByUserDto>> GetTransactionsByUser(
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the transactions by transaction type.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A list of transactions <seealso cref="TransactionByTransactionTypeDto"/>.</returns>
        Task<IEnumerable<TransactionByTransactionTypeDto>> GetTransactionsByTransactionType(
           CancellationToken cancellationToken = default);
    }
}
