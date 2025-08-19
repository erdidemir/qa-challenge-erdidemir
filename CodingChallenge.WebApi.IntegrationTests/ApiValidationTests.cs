using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace CodingChallenge.WebApi.IntegrationTests
{
    [Collection("Integration Tests")]
    public class ApiValidationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GetTransactions_ShouldReturnCorrectResponseStructure()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/v1/Transactions");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
            }
        }

        [Fact]
        public async Task GetUsers_ShouldReturnCorrectResponseStructure()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/v1/Users");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
            }
        }

        [Fact]
        public async Task AddTransaction_ShouldReturnCorrectResponseStructure()
        {
            // Arrange
            var addTransactionDto = new
            {
                UserId = "ValidationTestUser",
                TransactionType = "Credit",
                TransactionAmount = 500.00m
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync("/api/v1/Transactions", addTransactionDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            
            // Validate that response contains a numeric ID
            var transactionId = int.Parse(content);
            transactionId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task AddUser_ShouldReturnCorrectResponseStructure()
        {
            // Arrange
            var addUserDto = new
            {
                UserId = $"ValidationTestUser{Guid.NewGuid():N}",
                UserName = "Validation Test User",
                Email = $"validation{Guid.NewGuid():N}@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            // Act
            var response = await HttpClient.PostAsJsonAsync("/api/v1/Users", addUserDto);

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
            
            // Validate that response contains a numeric ID
            var userId = int.Parse(content);
            userId.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GetTransactionById_ShouldReturnCorrectResponseStructure()
        {
            // Arrange
            const int transactionId = 1;

            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions/{transactionId}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                var rootElement = jsonDocument.RootElement;
                rootElement.ValueKind.Should().Be(JsonValueKind.Object);
                
                // Validate required properties
                rootElement.TryGetProperty("transactionId", out _).Should().BeTrue();
                rootElement.TryGetProperty("userId", out _).Should().BeTrue();
                rootElement.TryGetProperty("transactionType", out _).Should().BeTrue();
                rootElement.TryGetProperty("transactionAmount", out _).Should().BeTrue();
            }
        }

        [Fact]
        public async Task GetUserById_ShouldReturnCorrectResponseStructure()
        {
            // Arrange
            const int userId = 1;

            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Users/{userId}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                var rootElement = jsonDocument.RootElement;
                rootElement.ValueKind.Should().Be(JsonValueKind.Object);
                
                // Validate required properties
                rootElement.TryGetProperty("id", out _).Should().BeTrue();
                rootElement.TryGetProperty("userId", out _).Should().BeTrue();
                rootElement.TryGetProperty("userName", out _).Should().BeTrue();
                rootElement.TryGetProperty("email", out _).Should().BeTrue();
            }
        }

        [Fact]
        public async Task GetHighVolumeTransactions_ShouldReturnCorrectResponseStructure()
        {
            // Arrange
            const decimal thresholdAmount = 1000m;

            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions/HighVolumeTransactions/{thresholdAmount}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
            }
        }

        [Fact]
        public async Task GetTransactionsByTransactionType_ShouldReturnCorrectResponseStructure()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/v1/Transactions/GroupByTransactionType");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
                
                // Validate array elements structure
                var array = jsonDocument.RootElement;
                if (array.GetArrayLength() > 0)
                {
                    var firstElement = array[0];
                    firstElement.TryGetProperty("transactionType", out _).Should().BeTrue();
                    firstElement.TryGetProperty("totalTransactionAmount", out _).Should().BeTrue();
                }
            }
        }

        [Fact]
        public async Task GetTransactionsByUser_ShouldReturnCorrectResponseStructure()
        {
            // Act
            var response = await HttpClient.GetAsync("/api/v1/Transactions/GroupByUser");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                content.Should().NotBeNullOrEmpty();
                
                // Validate JSON structure
                var jsonDocument = JsonDocument.Parse(content);
                jsonDocument.RootElement.ValueKind.Should().Be(JsonValueKind.Array);
                
                // Validate array elements structure
                var array = jsonDocument.RootElement;
                if (array.GetArrayLength() > 0)
                {
                    var firstElement = array[0];
                    firstElement.TryGetProperty("userId", out _).Should().BeTrue();
                    firstElement.TryGetProperty("totalTransactionAmount", out _).Should().BeTrue();
                }
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetTransactions_ShouldReturnBadRequest_ForInvalidPageNumber(int invalidPageNumber)
        {
            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions?pageNumber={invalidPageNumber}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(501)]
        public async Task GetTransactions_ShouldReturnBadRequest_ForInvalidPageSize(int invalidPageSize)
        {
            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions?pageSize={invalidPageSize}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetTransactionById_ShouldReturnBadRequest_ForInvalidId(int invalidId)
        {
            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions/{invalidId}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task GetUserById_ShouldReturnBadRequest_ForInvalidId(int invalidId)
        {
            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Users/{invalidId}");

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}

