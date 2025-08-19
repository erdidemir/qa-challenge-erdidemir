using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace CodingChallenge.WebApi.IntegrationTests
{
    [Collection("Integration Tests")]
    public class PerformanceTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        [Fact]
        public async Task GetTransactions_ShouldHandleConcurrentRequests_UnderLoad()
        {
            // Arrange
            const int concurrentRequests = 10;
            var tasks = new List<Task<HttpResponseMessage>>();

            // Act - Simulate concurrent requests
            for (int i = 0; i < concurrentRequests; i++)
            {
                tasks.Add(HttpClient.GetAsync("/api/v1/Transactions"));
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().HaveCount(concurrentRequests);
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetUsers_ShouldHandleConcurrentRequests_UnderLoad()
        {
            // Arrange
            const int concurrentRequests = 10;
            var tasks = new List<Task<HttpResponseMessage>>();

            // Act - Simulate concurrent requests
            for (int i = 0; i < concurrentRequests; i++)
            {
                tasks.Add(HttpClient.GetAsync("/api/v1/Users"));
            }

            var responses = await Task.WhenAll(tasks);

            // Assert
            responses.Should().HaveCount(concurrentRequests);
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode || r.StatusCode == HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task AddTransaction_ShouldHandleMultipleRequests_Sequentially()
        {
            // Arrange
            const int requestCount = 5;
            var addTransactionDto = new
            {
                UserId = "PerfTestUser",
                TransactionType = "Debit",
                TransactionAmount = 100.00m
            };

            var responses = new List<HttpResponseMessage>();

            // Act - Sequential requests
            for (int i = 0; i < requestCount; i++)
            {
                var response = await HttpClient.PostAsJsonAsync("/api/v1/Transactions", addTransactionDto);
                responses.Add(response);
            }

            // Assert
            responses.Should().HaveCount(requestCount);
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode);
        }

        [Fact]
        public async Task AddUser_ShouldHandleMultipleRequests_Sequentially()
        {
            // Arrange
            const int requestCount = 5;
            var addUserDto = new
            {
                UserId = $"PerfTestUser{Guid.NewGuid():N}",
                UserName = "Performance Test User",
                Email = $"perftest{Guid.NewGuid():N}@test.com",
                PhoneNumber = "1234567890",
                IsActive = true
            };

            var responses = new List<HttpResponseMessage>();

            // Act - Sequential requests
            for (int i = 0; i < requestCount; i++)
            {
                var response = await HttpClient.PostAsJsonAsync("/api/v1/Users", addUserDto);
                responses.Add(response);
            }

            // Assert
            responses.Should().HaveCount(requestCount);
            responses.Should().OnlyContain(r => r.IsSuccessStatusCode);
        }

        [Fact]
        public async Task GetHighVolumeTransactions_ShouldReturnResults_WithinReasonableTime()
        {
            // Arrange
            const decimal thresholdAmount = 1000m;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var response = await HttpClient.GetAsync($"/api/v1/Transactions/HighVolumeTransactions/{thresholdAmount}");
            stopwatch.Stop();

            // Assert
            response.Should().NotBeNull();
            response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NotFound);
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds
        }

        [Fact]
        public async Task GetTransactionsByTransactionType_ShouldReturnResults_WithinReasonableTime()
        {
            // Arrange
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var response = await HttpClient.GetAsync("/api/v1/Transactions/GroupByTransactionType");
            stopwatch.Stop();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000); // Should complete within 3 seconds
        }

        [Fact]
        public async Task GetTransactionsByUser_ShouldReturnResults_WithinReasonableTime()
        {
            // Arrange
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            // Act
            var response = await HttpClient.GetAsync("/api/v1/Transactions/GroupByUser");
            stopwatch.Stop();

            // Assert
            response.Should().NotBeNull();
            response.IsSuccessStatusCode.Should().BeTrue();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(3000); // Should complete within 3 seconds
        }
    }
}
