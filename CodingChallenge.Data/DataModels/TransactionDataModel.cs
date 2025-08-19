using CodingChallenge.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.DataModels
{
    /// <summary>
    /// The transaction data model.
    /// </summary>
    public class TransactionDataModel : BaseDataModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id value.</value>
        [Required]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the transaction type.
        /// </summary>
        /// <value>The transaction type value.</value>
        [Required]
        public required TransactionTypes TransactionType { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount value.</value>
        [Required]
        public required decimal Amount { get; set; }
    }
}
