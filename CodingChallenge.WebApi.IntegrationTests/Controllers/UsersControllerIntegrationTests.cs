using CodingChallenge.Dtos;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodingChallenge.WebApi.IntegrationTests.Controllers
{
    [Collection("Integration Tests")]
    [TestCaseOrderer("CodingChallenge.WebApi.IntegrationTests.PriorityOrderer", "CodingChallenge.WebApi.IntegrationTests")]
    public class UsersControllerIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Fact, TestPriority(1)]
        public async Task InvokingCreateUserApiEndPoint_ShouldReturn201StatusCode_AndUserId_WhenUserAddedSuccessfully()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "TestUser1",
                UserName = "Test User 1",
                Email = "testuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // Act
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Fact, TestPriority(2)]
        public async Task InvokingCreateUserApiEndPoint_ShouldReturn400StatusCode_WhenInvalidDataProvided()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "", // Invalid: empty UserId
                UserName = "", // Invalid: empty UserName
                Email = "invalid-email", // Invalid: wrong email format
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // Act
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }

        [Fact, TestPriority(3)]
        public async Task InvokingGetUsersApiEndPoint_ShouldReturn200StatusCode_AndUsersList_WhenUsersExist()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto1 = new()
            {
                UserId = "TestUser1",
                UserName = "Test User 1",
                Email = "testuser1@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            AddOrUpdateUserDto addUserDto2 = new()
            {
                UserId = "TestUser2",
                UserName = "Test User 2",
                Email = "testuser2@test.com",
                PhoneNumber = "0987654321",
                IsActive = false
            };

            // First create users
            HttpResponseMessage createResponse1 = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto1);
            createResponse1.StatusCode.Should().Be(HttpStatusCode.Created);

            HttpResponseMessage createResponse2 = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto2);
            createResponse2.StatusCode.Should().Be(HttpStatusCode.Created);

            const int pageNumber = 1;
            const int pageSize = 10;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetUsers, pageNumber, pageSize);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                var users = JsonSerializer.Deserialize<IEnumerable<UserDto>>(responseData, jsonSerializerOptions);
                users.Should().NotBeNull();
            }
        }

        [Fact, TestPriority(4)]
        public async Task InvokingGetUsersApiEndPoint_ShouldReturn404StatusCode_WhenNoUsersExist()
        {
            // Arrange
            const int pageNumber = 999;
            const int pageSize = 10;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetUsers, pageNumber, pageSize);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact, TestPriority(5)]
        public async Task InvokingGetUserByIdApiEndPoint_ShouldReturn200StatusCode_AndUserDetails_WhenUserExists()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "TestUserForGetById",
                UserName = "Test User For Get By Id",
                Email = "testuserforgetbyid@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // First create a user
            HttpResponseMessage createResponse = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            string userId = addUserDto.UserId;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                var user = JsonSerializer.Deserialize<UserDto>(responseData, jsonSerializerOptions);
                user.Should().NotBeNull();
                user!.UserId.Should().Be(userId);
            }
        }

        [Fact, TestPriority(6)]
        public async Task InvokingGetUserByIdApiEndPoint_ShouldReturn404StatusCode_WhenUserDoesNotExist()
        {
            // Arrange
            const string userId = "NonExistentUser";

            // Act
            string requestUri = string.Format(ApiEndpoints.GetUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact, TestPriority(7)]
        public async Task InvokingUpdateUserApiEndPoint_ShouldReturn204StatusCode_WhenUserUpdatedSuccessfully()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "TestUserForUpdate",
                UserName = "Test User For Update",
                Email = "testuserforupdate@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // First create a user
            HttpResponseMessage createResponse = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "UpdatedTestUser",
                UserName = "Updated Test User",
                Email = "updatedtestuser@test.com",
                PhoneNumber = "0987654321",
                IsActive = false
            };
            string userId = addUserDto.UserId;

            // Act
            string requestUri = string.Format(ApiEndpoints.UpdateUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PutAsJsonAsync(requestUri, updateUserDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact, TestPriority(8)]
        public async Task InvokingUpdateUserApiEndPoint_ShouldReturn404StatusCode_WhenUserDoesNotExist()
        {
            // Arrange
            AddOrUpdateUserDto updateUserDto = new()
            {
                UserId = "NonExistentUser",
                UserName = "Updated Test User",
                Email = "updatedtestuser@test.com",
                PhoneNumber = "0987654321",
                IsActive = false
            };
            const string userId = "NonExistentUser";

            // Act
            string requestUri = string.Format(ApiEndpoints.UpdateUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PutAsJsonAsync(requestUri, updateUserDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact, TestPriority(9)]
        public async Task InvokingDeleteUserApiEndPoint_ShouldReturn204StatusCode_WhenUserDeletedSuccessfully()
        {
            // Arrange
            AddOrUpdateUserDto addUserDto = new()
            {
                UserId = "TestUserForDelete",
                UserName = "Test User For Delete",
                Email = "testuserfordelete@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // First create a user
            HttpResponseMessage createResponse = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.CreateUser, addUserDto);
            createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

            string userId = addUserDto.UserId;

            // Act
            string requestUri = string.Format(ApiEndpoints.DeleteUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.DeleteAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Fact, TestPriority(10)]
        public async Task InvokingDeleteUserApiEndPoint_ShouldReturn404StatusCode_WhenUserDoesNotExist()
        {
            // Arrange
            const string userId = "NonExistentUser";

            // Act
            string requestUri = string.Format(ApiEndpoints.DeleteUserById, userId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.DeleteAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Theory, TestPriority(11)]
        [InlineData(0, 10)] // Invalid page number
        [InlineData(1, 0)] // Invalid page size
        [InlineData(1, 501)] // Page size exceeds maximum
        public async Task InvokingGetUsersApiEndPoint_ShouldReturn400StatusCode_WhenInvalidPaginationParametersProvided(
            int pageNumber, int pageSize)
        {
            // Act
            string requestUri = string.Format(ApiEndpoints.GetUsers, pageNumber, pageSize);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            }
        }
    }
}
