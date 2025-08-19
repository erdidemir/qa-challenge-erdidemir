using AutoMapper;
using CodingChallenge.Data;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service
{
    /// <summary>
    /// The implementation of <seealso cref="IUserService"/> which define user operation.
    /// </summary>
    /// <param name="codingChallengeDbContext">The instance of <seealso cref="ICodingChallengeDbContext"/></param>
    /// <param name="mapper">The instance of <seealso cref="IMapper"/></param>
    public class UserService(
        ICodingChallengeDbContext codingChallengeDbContext,
        IMapper mapper) : IUserService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<UserDto>> GetUsers(
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            IEnumerable<UserDataModel> userDataModels = await codingChallengeDbContext
                .Users
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return mapper.Map<IEnumerable<UserDto>>(userDataModels);
        }

        /// <inheritdoc />
        public async Task<UserDto?> GetUserById(
            int userId,
            CancellationToken cancellationToken = default)
        {
            UserDataModel? userDataModel = await codingChallengeDbContext
                .Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            return mapper.Map<UserDto?>(userDataModel);
        }

        /// <inheritdoc />
        public async Task<int> AddUser(
            AddOrUpdateUserDto addUserDto,
            CancellationToken cancellationToken = default)
        {
            UserDataModel userDataModel = mapper.Map<UserDataModel>(addUserDto);
            userDataModel.CreatedAt = DateTime.UtcNow;
            userDataModel.UpdatedAt = DateTime.UtcNow;

            await codingChallengeDbContext
                .Users
                .AddAsync(userDataModel, cancellationToken);

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return userDataModel.Id;
        }

        /// <inheritdoc />
        public async Task<bool> UpdateUser(
            int userId,
            AddOrUpdateUserDto updateUserDto,
            CancellationToken cancellationToken = default)
        {
            UserDataModel? userDataModel = await codingChallengeDbContext
                .Users
                .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (userDataModel is null)
            {
                return false;
            }

            mapper.Map(updateUserDto, userDataModel);
            userDataModel.UpdatedAt = DateTime.UtcNow;

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteUser(
            int userId,
            CancellationToken cancellationToken = default)
        {
            UserDataModel? userDataModel = await codingChallengeDbContext
                .Users
                .SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (userDataModel is null)
            {
                return false;
            }

            codingChallengeDbContext
                .Users
                .Remove(userDataModel);

            await codingChallengeDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
