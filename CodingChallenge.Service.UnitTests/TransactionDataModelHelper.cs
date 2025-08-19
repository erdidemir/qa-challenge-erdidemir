using CodingChallenge.Common.Enums;
using CodingChallenge.Data.DataModels;

namespace CodingChallenge.Service.UnitTests
{
    public static class TransactionDataModelHelper
    {
        public static IList<TransactionDataModel> CreateTransactionDataModels(int numberOfRecords)
        {
            var transactionDataModels = new List<TransactionDataModel>();

            for (int i = 1; i <= numberOfRecords; i++)
            {
                TransactionDataModel debitTransactionDataModel = new()
                {
                    UserId = $"TestUser{i}",
                    Amount = 1000 * i,
                    TransactionType = TransactionTypes.Debit,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                transactionDataModels.Add(debitTransactionDataModel);
                TransactionDataModel creditTransactionDataModel = new()
                {
                    UserId = $"TestUser{i}",
                    Amount = 1000 * i,
                    TransactionType = TransactionTypes.Credit,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                };
                transactionDataModels.Add(creditTransactionDataModel);
            }

            return transactionDataModels;
        }
    }
}
