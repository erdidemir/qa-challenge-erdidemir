using CodingChallenge.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.Abstraction.DataModels
{
    /// <summary>
    /// The transaction data model.
    /// </summary>
    public class TransactionDataModel : BaseDataModel
    {
        /// <summary>
        /// The UserId value.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// The TransactionType value
        /// </summary>
        [Required]
        public required TransactionTypes TransactionType { get; set; }

        /// <summary>
        /// The Amount value.
        /// </summary>
        [Required]
        public required decimal Amount { get; set; }
    }
}
