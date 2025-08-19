namespace CodingChallenge.WebApi.IntegrationTests
{
    public static class ApiEndpoints
    {
        public static string AddTransaction => "api/v1/Transactions";

        public static string UpdateTransactionById => "api/v1/Transactions/{0}";

        public static string DeleteTransactionById => "api/v1/Transactions/{0}";

        public static string GetTransactions => "api/v1/Transactions";

        public static string GetTransactionById => "api/v1/Transactions/{0}";

        public static string GetHighVolumeTransactions => "api/v1/Transactions/HighVolumeTransactions/{0}";

        public static string GetTransactionsByTransactionType => "api/v1/Transactions/GroupByTransactionType";

        public static string GetTransactionsByUser => "api/v1/Transactions/GroupByUser";
    }
}
