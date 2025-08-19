using CodingChallenge.Common.Constants;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallenge.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class UsersController(
        IUserService userService) : ControllerBase
    {
        /// <summary>
        /// Get the users.
        /// </summary>
        /// <param name="pageNumber">The page number value.</param>
        /// <param name="pageSize">The page size value.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A list of users <seealso cref="UserDto"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUsers(
            [FromQuery] int pageNumber = ApplicationConstants.TransactionDefaultPageNumber,
            [FromQuery] int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            if (pageNumber <= 0)
            {
                return BadRequest(SharedResources.InvalidPageNumberErrorMessage);
            }

            if (pageSize <= 0 || pageSize > ApplicationConstants.TransactionMaxPageSize)
            {
                return BadRequest(SharedResources.InvalidPageSizeErrorMessage);
            }

            IEnumerable<UserDto> users = await userService.GetUsers(
                pageNumber,
                pageSize,
                cancellationToken);

            if (users is null || !users.Any())
            {
                return NotFound(SharedResources.TransactionsNotFoundErrorMessage);
            }

            return Ok(users);
        }

        /// <summary>
        /// Get a user by ID.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The requested user <seealso cref="UserDto"/>.</returns>
        [HttpGet("{userId}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetUserById(
            [FromRoute] int userId,
            CancellationToken cancellationToken = default)
        {
            if (userId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            UserDto? user = await userService.GetUserById(
                userId,
                cancellationToken);

            if (user is null)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, userId));
            }

            return Ok(user);
        }

        /// <summary>
        /// Add a User.
        /// </summary>
        /// <param name="addUserDto">The user <seealso cref="AddOrUpdateUserDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The Id value of added user.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddUser(
            [FromBody] AddOrUpdateUserDto addUserDto,
            CancellationToken cancellationToken = default)
        {
            int userId = await userService.AddUser(
                addUserDto,
                cancellationToken);

            return Created($"/api/v1/Users/{userId}", userId);
        }

        /// <summary>
        /// Update the existing User By Id.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="updateUserDto">The user <seealso cref="AddOrUpdateUserDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        [HttpPut("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateUser(
            [FromRoute] int userId,
            [FromBody] AddOrUpdateUserDto updateUserDto,
            CancellationToken cancellationToken = default)
        {
            if (userId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            bool isSuccess = await userService.UpdateUser(
                userId,
                updateUserDto,
                cancellationToken);

            if (!isSuccess)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, userId));
            }

            return NoContent();
        }

        /// <summary>
        /// Delete the existing User By Id.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        [HttpDelete("{userId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteUser(
           [FromRoute] int userId,
           CancellationToken cancellationToken = default)
        {
            if (userId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            bool isSuccess = await userService.DeleteUser(
                userId,
                cancellationToken);

            if (!isSuccess)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, userId));
            }

            return NoContent();
        }
    }
}
