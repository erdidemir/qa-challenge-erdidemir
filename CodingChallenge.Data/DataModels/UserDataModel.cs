using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Data.DataModels
{
    /// <summary>
    /// The user data model.
    /// </summary>
    public class UserDataModel : BaseDataModel
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id value.</value>
        [Required]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        /// <value>The user name value.</value>
        [Required]
        [MaxLength(255)]
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email value.</value>
        [Required]
        [MaxLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>The phone number value.</value>
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        /// <value>The is active value.</value>
        public bool IsActive { get; set; } = true;
    }
}
