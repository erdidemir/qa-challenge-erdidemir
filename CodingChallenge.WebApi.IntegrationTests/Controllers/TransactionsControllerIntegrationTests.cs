using CodingChallenge.Common.Enums;
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
    public class TransactionsControllerIntegrationTests(CustomWebApplicationFactory factory) : BaseIntegrationTest(factory)
    {
        private readonly JsonSerializerOptions jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        [Theory, TestPriority(1)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task InvokingAddTransactionApiEndPoint_ShouldReturn201StatusCode_AndTransactionId_WhenTransactionAddedSuccessfully(
            TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto addTransactionDto = new()
            {
                TransactionAmount = 1599,
                TransactionType = transactionType,
                UserId = "TestUser1"
            };

            // Act
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PostAsJsonAsync(ApiEndpoints.AddTransaction, addTransactionDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();
            }
        }

        [Theory, TestPriority(2)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task InvokingUpdateTransactionApiEndPoint_ShouldReturn204StatusCode_WhenTransactionUpdatedSuccessfully(
            TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                TransactionAmount = 15999,
                TransactionType = transactionType,
                UserId = "UpdatedTestUser1"
            };
            int transactionId = base.CodingChallengeDbContext
                .Transactions
                .Where(x => x.TransactionType == transactionType).Last().Id;

            // Act
            string requestUri = string.Format(ApiEndpoints.UpdateTransactionById, transactionId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PutAsJsonAsync(
                requestUri,
                updateTransactionDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NoContent);
            }
        }

        [Theory, TestPriority(3)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task InvokingUpdateTransactionApiEndPoint_ShouldReturn404StatusCode_WhenMatchingTransactionRecordNotFound(
           TransactionTypes transactionType)
        {
            // Arrange
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                TransactionAmount = 15999,
                TransactionType = transactionType,
                UserId = "UpdatedTestUser1"
            };
            const int transactionId = int.MaxValue;

            // Act
            string requestUri = string.Format(ApiEndpoints.UpdateTransactionById, transactionId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.PutAsJsonAsync(
                requestUri,
                updateTransactionDto);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }
  
        [Fact, TestPriority(4)]
        public async Task InvokingGetTransactionByIdApiEndPoint_ShouldReturn404StatusCode_WhenMatchingTransactionRecordNotFound()
        {
            // Arrange
            const int transactionId = int.MaxValue;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetTransactionById, transactionId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Theory, TestPriority(5)]
        [InlineData(TransactionTypes.Debit)]
        [InlineData(TransactionTypes.Credit)]
        public async Task InvokingGetTransactionByIdApiEndPoint_ShouldReturn200StatusCode_WhenMatchingTransactionRecordIsFound(
            TransactionTypes transactionType)
        {
            // Arrange
            int transactionId = base.CodingChallengeDbContext
                .Transactions
                .Where(x => x.TransactionType == transactionType).Last().Id;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetTransactionById, transactionId);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                TransactionDto? transactionDto = JsonSerializer.Deserialize<TransactionDto?>(responseData, jsonSerializerOptions);
                transactionDto.Should().NotBeNull();
            }
        }

        [Fact, TestPriority(6)]
        public async Task InvokingGetTransactionsApiEndPoint_ShouldRetur200StatusCode_WhenTransactionsDataAvailable()
        {
            // Arrange
            
            // Act
            string requestUri = ApiEndpoints.GetTransactions;
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                IEnumerable<TransactionDto>? transactionDtos = JsonSerializer.Deserialize<IEnumerable<TransactionDto>?>(responseData, jsonSerializerOptions);
                transactionDtos.Should().NotBeNull();
                transactionDtos.Should().NotBeEmpty();
            }
        }

        [Fact, TestPriority(7)]
        public async Task InvokingGetTransactionsApiEndPoint_ShouldRetur404StatusCode_WhenTransactionsDataNotReturned()
        {
            // Arrange

            // Act
            string requestUri = $"{ApiEndpoints.GetTransactions}?pageNumber=2";
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact, TestPriority(8)]
        public async Task InvokingGetHighVolumeTransactionsApiEndPoint_ShouldRetur200StatusCode_WhenTransactionsDataAvailable()
        {
            // Arrange
            const decimal thresholdAmount = 1500;
            
            // Act
            string requestUri = string.Format(ApiEndpoints.GetHighVolumeTransactions, thresholdAmount);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                IEnumerable<TransactionDto>? transactionDtos = JsonSerializer.Deserialize<IEnumerable<TransactionDto>?>(responseData, jsonSerializerOptions);
                transactionDtos.Should().NotBeNull();
                transactionDtos.Should().NotBeEmpty();
            }
        }

        [Fact, TestPriority(9)]
        public async Task InvokingGetHighVolumeTransactionsApiEndPoint_ShouldRetur404StatusCode_WhenTransactionsDataNotReturned()
        {
            // Arrange
            const decimal thresholdAmount = decimal.MaxValue;

            // Act
            string requestUri = string.Format(ApiEndpoints.GetHighVolumeTransactions, thresholdAmount);
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.NotFound);
            }
        }

        [Fact, TestPriority(10)]
        public async Task InvokingGetTransactionsByTransactionTypeApiEndPoint_ShouldRetur200StatusCode_WhenTransactionsDataAvailable()
        {
            // Arrange
            
            // Act
            string requestUri = ApiEndpoints.GetTransactionsByTransactionType;
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();

                IEnumerable<TransactionByTransactionTypeDto>? transactionByTransactionTypeDtos = JsonSerializer.Deserialize<IEnumerable<TransactionByTransactionTypeDto>?>(responseData, jsonSerializerOptions);
                transactionByTransactionTypeDtos.Should().NotBeNull();
                transactionByTransactionTypeDtos.Should().NotBeEmpty();
            }
        }

        [Fact, TestPriority(11)]
        public async Task InvokingGetTransactionsByUserApiEndPoint_ShouldRetur200StatusCode_WhenTransactionsDataAvailable()
        {
            // Arrange

            // Act
            string requestUri = ApiEndpoints.GetTransactionsByUser;
            HttpResponseMessage httpResponseMessage = await base.HttpClient.GetAsync(
                requestUri);

            // Assert
            using (new AssertionScope())
            {
                httpResponseMessage.Should().NotBeNull();
                httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);

                string responseData = await httpResponseMessage.Content.ReadAsStringAsync();
                responseData.Should().NotBeNullOrWhiteSpace();
                IEnumerable<TransactionByUserDto>? transactionByUserDtos = JsonSerializer.Deserialize<IEnumerable<TransactionByUserDto>?>(responseData, jsonSerializerOptions);
                transactionByUserDtos.Should().NotBeNull();
                transactionByUserDtos.Should().NotBeEmpty();
            }
        }
    }
}
