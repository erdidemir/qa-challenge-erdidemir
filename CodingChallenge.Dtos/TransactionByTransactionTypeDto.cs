using CodingChallenge.Common.Enums;
using System.Text.Json.Serialization;

namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The Transaction By Transaction Type Dto
    /// </summary>
    public class TransactionByTransactionTypeDto
    {
        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        /// <value>The transaction type value.</value>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TransactionTypes TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the total transaction amount.
        /// </summary>
        /// <value>The total transaction amount value.</value>
        public required decimal TotalTransactionAmount { get; set; }
    }
}
