using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.DataModels
{
    /// <summary>
    /// The base data model
    /// </summary>
    public abstract class BaseDataModel
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id value.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        /// <value>The created at value.</value>
        [Required]
        public required DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        /// <value>The updated at value.</value>
        [Required]
        public required DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        /// <value>The row version value.</value>
        [ConcurrencyCheck]
        [Timestamp]
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
    }
}
