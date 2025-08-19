using System.ComponentModel.DataAnnotations;

namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The User Dto.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public required string UserName { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        public required string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        public string? PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the is active.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the created at.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the updated at.
        /// </summary>
        public DateTime UpdatedAt { get; set; }
    }
}
