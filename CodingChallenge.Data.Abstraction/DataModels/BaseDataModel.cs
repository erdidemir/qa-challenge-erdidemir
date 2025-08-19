using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.Abstraction.DataModels
{

    /// <summary>
    /// The base data model
    /// </summary>
    public abstract class BaseDataModel
    {
        /// <summary>
        /// The Id value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The CreatedAt value.
        /// </summary>
        [Required]
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// The UpdatedAt value.
        /// </summary>
        [Required]
        public required DateTime UpdatedAt { get; set; }
    }
}
