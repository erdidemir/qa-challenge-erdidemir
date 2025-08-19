using CodingChallenge.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The Add Or Update Transaction Dto.
    /// </summary>
    public class AddOrUpdateTransactionDto
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id value.</value>
        [CustomRequiredAttribute(typeof(string))]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        /// <value>The transaction type value.</value>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required TransactionTypes TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the transaction amount.
        /// </summary>
        /// <value>The transaction amount value.</value>
        public required decimal TransactionAmount { get; set; }
     }
}
