using AutoMapper;
using CodingChallenge.Common.Constants;
using CodingChallenge.Data;
using CodingChallenge.Data.Abstraction;
using CodingChallenge.Data.DataModels;
using CodingChallenge.Dtos;
using CodingChallenge.Service;
using CodingChallenge.Service.MappingProfiles;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;

namespace CodingChallenge.Service.UnitTests
{
    [TestCaseOrderer("CodingChallenge.Service.UnitTests.PriorityOrderer", "CodingChallenge.Service.UnitTests")]
    public class UserServiceUnitTests : IClassFixture<InMemoryCodingChallengeDbContext>
    {
        private readonly UserService _userServiceUnderTest;
        private readonly IMapper _mapper;
        private readonly ICodingChallengeDbContext _codingChallengeDbContext;

        public UserServiceUnitTests(InMemoryCodingChallengeDbContext inMemoryCodingChallengeDbContext)
        {
            _codingChallengeDbContext = inMemoryCodingChallengeDbContext.CodingChallengeDbContext;

            var userMappingProfile = new UserMappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(userMappingProfile));
            _mapper = new Mapper(configuration);

            _userServiceUnderTest = new UserService(
                _codingChallengeDbContext,
                _mapper);
        }

        [Fact, TestPriority(1)]
        public async Task CallingGetUsersMethod_ShouldReturnEmptyCollection_WhenNoUserDataAvailable()
        {
            // Arrange
            const int pageNumber = ApplicationConstants.TransactionDefaultPageNumber;
            const int pageSize = ApplicationConstants.TransactionDefaultPageSize;
            CancellationTokenSource tokenSource = new();

            // Act
            IEnumerable<UserDto> userDtos = await _userServiceUnderTest.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            userDtos.Should().BeEmpty();
        }

        [Fact, TestPriority(2)]
        public async Task CallingGetUsersMethod_ShouldReturnMappedCollection_WhenUserDataIsAvailable()
        {
            // Arrange
            const int pageNumber = 1;
            const int pageSize = 3;
            CancellationTokenSource tokenSource = new();

            await SeedUserDataForTests();

            // Act
            IEnumerable<UserDto> userDtos = await _userServiceUnderTest.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                userDtos.Should().NotBeEmpty();
                userDtos.Should().HaveCount(3);
            }
        }

        [Fact, TestPriority(3)]
        public async Task CallingGetUserByIdMethod_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();

            await SeedUserDataForTests();

            // Act
            UserDto? userDto = await _userServiceUnderTest.GetUserById(
                userId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                userDto.Should().NotBeNull();
                userDto!.Id.Should().Be(userId);
                userDto.UserId.Should().Be("TestUser1");
            }
        }

        [Fact, TestPriority(4)]
        public async Task CallingGetUserByIdMethod_ShouldReturnNull_WhenUserDoesNotExist()
        {
            // Arrange
            const int userId = 999;
            CancellationTokenSource tokenSource = new();

            // Act
            UserDto? userDto = await _userServiceUnderTest.GetUserById(
                userId,
                tokenSource.Token);

            // Assert
            userDto.Should().BeNull();
        }

        [Fact, TestPriority(5)]
        public async Task CallingAddUserMethod_ShouldReturnUserId_WhenUserAddedSuccessfully()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "NewUser1",
                UserName = "New User 1",
                Email = "newuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            CancellationTokenSource tokenSource = new();

            // Act
            int userId = await _userServiceUnderTest.AddUser(
                addUserDto,
                tokenSource.Token);

            // Assert
            userId.Should().BeGreaterThan(0);
        }

        [Fact, TestPriority(6)]
        public async Task CallingUpdateUserMethod_ShouldReturnTrue_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            const int userId = 1;
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "UpdatedUser1",
                UserName = "Updated User 1",
                Email = "updateduser1@test.com",
                PhoneNumber = "0987654321",
                IsActive = false
            };
            CancellationTokenSource tokenSource = new();

            await SeedUserDataForTests();

            // Act
            bool isSuccess = await _userServiceUnderTest.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token);

            // Assert
            isSuccess.Should().BeTrue();
        }

        [Fact, TestPriority(7)]
        public async Task CallingUpdateUserMethod_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            const int userId = 999;
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "NonExistentUser",
                UserName = "Non Existent User",
                Email = "nonexistent@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            CancellationTokenSource tokenSource = new();

            // Act
            bool isSuccess = await _userServiceUnderTest.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token);

            // Assert
            isSuccess.Should().BeFalse();
        }

        [Fact, TestPriority(8)]
        public async Task CallingDeleteUserMethod_ShouldReturnTrue_WhenUserDeletedSuccessfully()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();

            await SeedUserDataForTests();

            // Act
            bool isSuccess = await _userServiceUnderTest.DeleteUser(
                userId,
                tokenSource.Token);

            // Assert
            isSuccess.Should().BeTrue();
        }

        [Fact, TestPriority(9)]
        public async Task CallingDeleteUserMethod_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            // Arrange
            const int userId = 999;
            CancellationTokenSource tokenSource = new();

            // Act
            bool isSuccess = await _userServiceUnderTest.DeleteUser(
                userId,
                tokenSource.Token);

            // Assert
            isSuccess.Should().BeFalse();
        }

        private async Task SeedUserDataForTests()
        {
            if (_codingChallengeDbContext.Users.Any())
                return;

            var users = new List<UserDataModel>
            {
                new()
                {
                    Id = 1,
                    UserId = "TestUser1",
                    UserName = "Test User 1",
                    Email = "testuser1@test.com",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 2,
                    UserId = "TestUser2",
                    UserName = "Test User 2",
                    Email = "testuser2@test.com",
                    PhoneNumber = "0987654321",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new()
                {
                    Id = 3,
                    UserId = "TestUser3",
                    UserName = "Test User 3",
                    Email = "testuser3@test.com",
                    PhoneNumber = "5555555555",
                    IsActive = false,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await _codingChallengeDbContext.Users.AddRangeAsync(users);
            await _codingChallengeDbContext.SaveChangesAsync();
        }
    }
}
