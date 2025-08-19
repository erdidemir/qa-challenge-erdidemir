using CodingChallenge.Common.Constants;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using CodingChallenge.WebApi.Controllers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CodingChallenge.WebApi.UnitTests.Controllers
{
    public class UsersControllerUnitTests
    {
        public readonly UsersController _usersControllerUnderTest;
        private readonly Mock<IUserService> _mockIUserService = new();

        public UsersControllerUnitTests()
        {
            _usersControllerUnderTest = new UsersController(_mockIUserService.Object);
        }

        [Fact]
        public async Task CallingGetUsersMethod_ShouldReturnUsers_WhenInputValidationPassed_And_BackEndReturnData()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<UserDto> users =
            [
                new()
                {
                    Id = 1,
                    UserId = "TestUserId1",
                    UserName = "Test User 1",
                    Email = "testuser1@test.com",
                    PhoneNumber = "1234567890",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            ];

            // Setup
            _mockIUserService.Setup(x => x.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(users);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<OkObjectResult>();
                var okObjectResult = (OkObjectResult)actionResult;
                okObjectResult.Should().NotBeNull();
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.Value.Should().BeSameAs(users);

                // Verify
                _mockIUserService.Verify(x => x.GetUsers(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetUsersMethod_ShouldReturnNotFound_WhenInputValidationPassed_But_BackEndReturnNoData()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<UserDto> users = [];

            // Setup
            _mockIUserService.Setup(x => x.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(users);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUsers(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();
                notFoundObjectResult.Value.Should().NotBeNull();

                // Verify
                _mockIUserService.Verify(x => x.GetUsers(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CallingGetUsersMethod_ShouldReturnBadRequest_WhenPageNumberIsInvalid(int invalidPageNumber)
        {
            // Arrange
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUsers(
                invalidPageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.Value.Should().NotBeNull();
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(501)]
        public async Task CallingGetUsersMethod_ShouldReturnBadRequest_WhenPageSizeIsInvalid(int invalidPageSize)
        {
            // Arrange
            const int pageNumber = 1;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUsers(
                pageNumber,
                invalidPageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.Value.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task CallingGetUserByIdMethod_ShouldReturnUser_WhenInputValidationPassed_And_BackEndReturnData()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();
            UserDto user = new()
            {
                Id = userId,
                UserId = "TestUserId1",
                UserName = "Test User 1",
                Email = "testuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Setup
            _mockIUserService.Setup(x => x.GetUserById(
                userId,
                tokenSource.Token)).ReturnsAsync(user);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUserById(
                userId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<OkObjectResult>();
                var okObjectResult = (OkObjectResult)actionResult;
                okObjectResult.Should().NotBeNull();
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.Value.Should().BeSameAs(user);

                // Verify
                _mockIUserService.Verify(x => x.GetUserById(
                    userId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetUserByIdMethod_ShouldReturnNotFound_WhenInputValidationPassed_But_BackEndReturnNoData()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();
            UserDto? user = null;

            // Setup
            _mockIUserService.Setup(x => x.GetUserById(
                userId,
                tokenSource.Token)).ReturnsAsync(user);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUserById(
                userId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();
                notFoundObjectResult.Value.Should().NotBeNull();

                // Verify
                _mockIUserService.Verify(x => x.GetUserById(
                    userId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CallingGetUserByIdMethod_ShouldReturnBadRequest_WhenUserIdIsInvalid(int invalidUserId)
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.GetUserById(
                invalidUserId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.Value.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task CallingAddUserMethod_ShouldReturnCreated_WhenUserAddedSuccessfully()
        {
            // Arrange
            const int userId = 1;
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "TestUserId1",
                UserName = "Test User 1",
                Email = "testuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            CancellationTokenSource tokenSource = new();

            // Setup
            _mockIUserService.Setup(x => x.AddUser(
                addUserDto,
                tokenSource.Token)).ReturnsAsync(userId);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.AddUser(
                addUserDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<CreatedResult>();
                var createdAtActionResult = (CreatedResult)actionResult;
                createdAtActionResult.Should().NotBeNull();
                createdAtActionResult.Value.Should().NotBeNull();
                createdAtActionResult.Value.Should().Be(userId);

                // Verify
                _mockIUserService.Verify(x => x.AddUser(
                    addUserDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingUpdateUserMethod_ShouldReturnNoContent_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            const int userId = 1;
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "UpdatedUserId1",
                UserName = "Updated User 1",
                Email = "updateduser1@test.com",
                PhoneNumber = "0987654321",
                IsActive = false
            };
            CancellationTokenSource tokenSource = new();

            // Setup
            _mockIUserService.Setup(x => x.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token)).ReturnsAsync(true);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NoContentResult>();

                // Verify
                _mockIUserService.Verify(x => x.UpdateUser(
                    userId,
                    updateUserDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingUpdateUserMethod_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            const int userId = 1;
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "NonExistentUserId",
                UserName = "Non Existent User",
                Email = "nonexistent@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            CancellationTokenSource tokenSource = new();

            // Setup
            _mockIUserService.Setup(x => x.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token)).ReturnsAsync(false);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.UpdateUser(
                userId,
                updateUserDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();
                notFoundObjectResult.Value.Should().NotBeNull();

                // Verify
                _mockIUserService.Verify(x => x.UpdateUser(
                    userId,
                    updateUserDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CallingUpdateUserMethod_ShouldReturnBadRequest_WhenUserIdIsInvalid(int invalidUserId)
        {
            // Arrange
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "TestUserId1",
                UserName = "Test User 1",
                Email = "testuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.UpdateUser(
                invalidUserId,
                updateUserDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.Value.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task CallingDeleteUserMethod_ShouldReturnNoContent_WhenUserDeletedSuccessfully()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();

            // Setup
            _mockIUserService.Setup(x => x.DeleteUser(
                userId,
                tokenSource.Token)).ReturnsAsync(true);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.DeleteUser(
                userId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NoContentResult>();

                // Verify
                _mockIUserService.Verify(x => x.DeleteUser(
                    userId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingDeleteUserMethod_ShouldReturnNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            const int userId = 1;
            CancellationTokenSource tokenSource = new();

            // Setup
            _mockIUserService.Setup(x => x.DeleteUser(
                userId,
                tokenSource.Token)).ReturnsAsync(false);

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.DeleteUser(
                userId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();
                notFoundObjectResult.Value.Should().NotBeNull();

                // Verify
                _mockIUserService.Verify(x => x.DeleteUser(
                    userId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task CallingDeleteUserMethod_ShouldReturnBadRequest_WhenUserIdIsInvalid(int invalidUserId)
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _usersControllerUnderTest.DeleteUser(
                invalidUserId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();
                badRequestObjectResult.Value.Should().NotBeNull();
            }
        }
    }
}
