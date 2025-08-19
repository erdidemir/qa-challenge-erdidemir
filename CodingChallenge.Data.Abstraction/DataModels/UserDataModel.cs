using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.Abstraction.DataModels
{
    /// <summary>
    /// The user data model.
    /// </summary>
    public class UserDataModel : BaseDataModel
    {
        /// <summary>
        /// The UserId value.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// The UserName value.
        /// </summary>
        [Required]
        [MaxLength(255)]
        public required string UserName { get; set; }

        /// <summary>
        /// The Email value.
        /// </summary>
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// The PhoneNumber value.
        /// </summary>
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// The IsActive value.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
