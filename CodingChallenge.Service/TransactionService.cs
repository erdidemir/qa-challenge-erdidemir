using AutoMapper;
using CodingChallenge.Common.Constants;
using CodingChallenge.Data;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using CodingChallenge.Service.Factories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service
{
    /// <summary>
    /// The implementation of <seealso cref="ITransactionService"/> which define transaction operation.
    /// </summary>
    /// <param name="codingChallengeDbContext">The instance of <seealso cref="ICodingChallengeDbContext"/></param>
    /// <param name="mapper">The instance of <seealso cref="IMapper"/>.</param>
    /// <param name="transactionDataModelFactory">The instance of <seealso cref="ITransactionDataModelFactory"/>.</param>
    public class TransactionService(
        ICodingChallengeDbContext codingChallengeDbContext,
        IMapper mapper,
        ITransactionDataModelFactory transactionDataModelFactory) : ITransactionService
    {
        /// <inheritdoc />
        public async Task<int> AddTransaction(
            AddOrUpdateTransactionDto addTransactionDto,
            CancellationToken cancellationToken = default)
        {
            TransactionDataModel transactionDataModel = transactionDataModelFactory.CreateTransactionDataModel(addTransactionDto);
            
            await codingChallengeDbContext
                .Transactions
                .AddAsync(transactionDataModel, cancellationToken);

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return transactionDataModel.Id;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteTransaction(
            int transactionId,
            CancellationToken cancellationToken = default)
        {
            TransactionDataModel? transactionDataModel = await codingChallengeDbContext
               .Transactions
               .SingleOrDefaultAsync(x => x.Id == transactionId, cancellationToken);

            if (transactionDataModel is null)
            {
                return default;
            }

            codingChallengeDbContext
               .Transactions
               .Remove(transactionDataModel);

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> GetTransactionById(
            int transactionId,
            CancellationToken cancellationToken = default)
        {
            TransactionDataModel? transactionDataModel = await codingChallengeDbContext
                .Transactions
                .AsNoTracking()
                .SingleOrDefaultAsync(x=> x.Id == transactionId, cancellationToken);
            
            if (transactionDataModel is null)
            {
                return default;
            }
            
            // Not a Big Fan of AutoMapper, as I prefer Factory Pattern. Only Added this as request
            return mapper.Map<TransactionDto>(transactionDataModel);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionDto>> GetTransactions(
            int pageNuber = ApplicationConstants.TransactionDefaultPageNumber,
            int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionDataModel> transactionDataModels = codingChallengeDbContext
                .Transactions
                .AsNoTracking()
                .Skip((pageNuber - 1 ) * pageSize)
                .Take(pageSize);

            if (transactionDataModels is null || !transactionDataModels.Any())
            {
                return await Task.FromResult(Enumerable.Empty<TransactionDto>());
            }

            // Not a Big Fan of AutoMapper, as I prefer Factory Pattern. Only Added this as request
            return mapper.Map<IEnumerable<TransactionDto>>(transactionDataModels);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<TransactionDto>> GetHighVolumeTransactions(
            decimal thresholdamount,
            int pageNuber = ApplicationConstants.TransactionDefaultPageNumber,
            int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionDataModel> transactionDataModels = codingChallengeDbContext
                .Transactions
                .Where(x => x.Amount > thresholdamount)
                .AsNoTracking()
                .Skip((pageNuber - 1) * pageSize)
                .Take(pageSize);

            if (transactionDataModels is null || !transactionDataModels.Any())
            {
                return await Task.FromResult(Enumerable.Empty<TransactionDto>());
            }

            // Not a Big Fan of AutoMapper, as I prefer Factory Pattern. Only Added this as request
            return mapper.Map<IEnumerable<TransactionDto>>(transactionDataModels);
        }

        /// <inheritdoc />
        public async Task<bool> UpdateTransaction(
            int transactionId,
            AddOrUpdateTransactionDto updateTransactionDto,
            CancellationToken cancellationToken = default)
        {
            TransactionDataModel? transactionDataModel = await codingChallengeDbContext
                .Transactions
                .SingleOrDefaultAsync(x => x.Id == transactionId, cancellationToken);

            if (transactionDataModel is null)
            {
                return default;
            }

            _ = transactionDataModelFactory.UpdateTransactionDataModel(transactionDataModel, updateTransactionDto);

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
