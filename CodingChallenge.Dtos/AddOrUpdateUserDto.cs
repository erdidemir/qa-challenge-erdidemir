using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The Add Or Update User Dto.
    /// </summary>
    public class AddOrUpdateUserDto
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        [CustomRequiredAttribute(typeof(string))]
        [MaxLength(255)]
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        [CustomRequiredAttribute(typeof(string))]
        [MaxLength(255)]
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [CustomRequiredAttribute(typeof(string))]
        [MaxLength(255)]
        [EmailAddress]
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
