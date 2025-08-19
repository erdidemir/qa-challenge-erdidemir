using CodingChallenge.Data;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service
{
    /// <summary>
    /// The implementation of <seealso cref="ITransactionSummaryService"/> which define transaction summary operation.
    /// </summary>
    /// <param name="codingChallengeDbContext">The instance of <seealso cref="ICodingChallengeDbContext"/></param>
    public class TransactionSummaryService(
        ICodingChallengeDbContext codingChallengeDbContext) : ITransactionSummaryService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<TransactionByTransactionTypeDto>> GetTransactionsByTransactionType(
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = codingChallengeDbContext
                 .Transactions
                 .AsNoTracking()
                 .GroupBy(x => x.TransactionType).Select(y => new TransactionByTransactionTypeDto()
                 {
                     TransactionType = y.Key,
                     TotalTransactionAmount = y.Sum(z => z.Amount)
                 });

            return await Task.FromResult(transactionByTransactionTypeDtos);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionByUserDto>> GetTransactionsByUser(
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionByUserDto> transactionByUserDtos = codingChallengeDbContext
                 .Transactions
                 .AsNoTracking()
                 .GroupBy(x => x.UserId).Select(y => new TransactionByUserDto()
                 {
                     UserId = y.Key,
                     TotalTransactionAmount = y.Sum(z => z.Amount)
                 });

            return await Task.FromResult(transactionByUserDtos);
        }
    }
}
