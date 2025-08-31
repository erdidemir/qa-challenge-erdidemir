using CodingChallenge.Dtos;

namespace CodingChallenge.Service.Abstraction
{
    /// <summary>
    /// The user service interface.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of users.</returns>
        Task<IEnumerable<UserDto>> GetUsers(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the user by id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The user.</returns>
        Task<UserDto?> GetUserById(
            string userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="addUserDto">The add user dto.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The user id.</returns>
        Task<int> AddUser(
            AddOrUpdateUserDto addUserDto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="updateUserDto">The update user dto.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The success status.</returns>
        Task<bool> UpdateUser(
            string userId,
            AddOrUpdateUserDto updateUserDto,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes the user.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The success status.</returns>
        Task<bool> DeleteUser(
            string userId,
            CancellationToken cancellationToken = default);
    }
}
