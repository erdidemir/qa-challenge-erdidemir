using CodingChallenge.Common.Constants;
using CodingChallenge.Common.Enums;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using CodingChallenge.WebApi.Controllers;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CodingChallenge.WebApi.UnitTests.Controllers
{
    public class TransactionsControllerUnitTests
    {
        public readonly TransactionsController _transactionsControllerUnderTest;
        private readonly Mock<ITransactionService> _mockITransactionService = new();
        private readonly Mock<ITransactionSummaryService> _mockITransactionSummaryService = new();

        public TransactionsControllerUnitTests()
        {
            _transactionsControllerUnderTest = new TransactionsController(
               _mockITransactionService.Object,
               _mockITransactionSummaryService.Object);
        }

        [Fact]
        public async Task CallingGetTransactionsMethod_ShouldReturnTransactions_WhenInputValidationPassed_And_BackEndReturnData()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionDto> transactions =
            [
                new()
                {
                    TransactionId = 101,
                    UserId = "TestUserId1",
                    TransactionType = TransactionTypes.Debit,
                    TransactionAmount =  1095,
                    TransactionCreatedAt = DateTime.UtcNow,
                }
            ];

            // Setup
            _mockITransactionService.Setup(x => x.GetTransactions(
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(transactions);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactions(
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
                okObjectResult.Value.Should().BeSameAs(transactions);

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactions(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsMethod_ShouldReturnNotFound_WhenInputValidationPassed_But_BackEndReturnNoData()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionDto> transactions = [];

            // Setup
            _mockITransactionService.Setup(x => x.GetTransactions(
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(transactions);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactions(
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

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactions(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsMethod_ShouldReturnBadRequest_WhenInputPageSizeExceedsMaxLimit()
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = ApplicationConstants.TransactionMaxPageSize + 1;
            CancellationTokenSource tokenSource = new();
          
            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactions(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactions(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetTransactionsMethod_ShouldReturnBadRequest_WhenInputPageSizeIsInvalid(
            int pageSize)
        {
            // Arrange
            const int pageNumber = 2;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactions(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactions(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetTransactionsMethod_ShouldReturnBadRequest_WhenInputPageNumberIsInvalid(
           int pageNumber)
        {
            // Arrange
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactions(
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactions(
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Fact]
        public async Task CallingGetTransactionByIdMethod_ShouldReturnTransaction_WhenInputValidationPassed_And_BackEndReturnData()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            TransactionDto transaction = new()
            {
                TransactionId = transactionId,
                UserId = "TestUserId1",
                TransactionType = TransactionTypes.Debit,
                TransactionAmount = 1095,
                TransactionCreatedAt = DateTime.UtcNow,
            };

            // Setup
            _mockITransactionService.Setup(x => x.GetTransactionById(
                transactionId,
                tokenSource.Token)).ReturnsAsync(transaction);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionById(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<OkObjectResult>();
                var okObjectResult = (OkObjectResult)actionResult;
                okObjectResult.Should().NotBeNull();
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.Value.Should().BeSameAs(transaction);

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactionById(
                    transactionId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionByIdMethod_ShouldReturnNotFound_WhenInputValidationPassed_But_BackEndReturnNoData()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            TransactionDto? transaction = null;

            // Setup
            _mockITransactionService.Setup(x => x.GetTransactionById(
                transactionId,
                tokenSource.Token)).ReturnsAsync(transaction);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionById(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactionById(
                    transactionId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetTransactionByIdMethod_ShouldReturnBadRequest_WhenInputTransactionIdIsInvalid(
          int transactionId)
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionById(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetTransactionById(
                    transactionId,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Fact]
        public async Task CallingAddTransactionMethod_ShouldReturnTransactionId_WhenTransactionAddedSuccessfully()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            AddOrUpdateTransactionDto addTransactionDto = new()
            {
                UserId = "TestUserId1",
                TransactionType = TransactionTypes.Debit,
                TransactionAmount = 1095,
            };

            // Setup
            _mockITransactionService.Setup(x => x.AddTransaction(
                addTransactionDto,
                tokenSource.Token)).ReturnsAsync(transactionId);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.AddTransaction(
                addTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<CreatedResult>();
                var createdResult = (CreatedResult)actionResult;
                createdResult.Should().NotBeNull();
                createdResult.Value.Should().NotBeNull();
                createdResult.Value.Should().Be(transactionId);
                createdResult.Location.Should().NotBeNull();
                createdResult.Location.Should().Be($"/api/v1/Transactions/{transactionId}");

                // Verify
                _mockITransactionService.Verify(x => x.AddTransaction(
                    addTransactionDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingUpdateTransactionMethod_ShouldReturnNoContent_WhenTransactionUpdatedSuccessfully()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUserId1",
                TransactionType = TransactionTypes.Debit,
                TransactionAmount = 1095,
            };
            const bool isSuccess = true;

            // Setup
            _mockITransactionService.Setup(x => x.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token)).ReturnsAsync(isSuccess);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NoContentResult>();
                var noContentResult = (NoContentResult)actionResult;
                noContentResult.Should().NotBeNull();
              
                // Verify
                _mockITransactionService.Verify(x => x.UpdateTransaction(
                    transactionId,
                    updateTransactionDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingUpdateTransactionMethod_ShouldReturnNotFound_WhenTransactionUpdateIsFailed()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUserId1",
                TransactionType = TransactionTypes.Debit,
                TransactionAmount = 1095,
            };
            const bool isSuccess = false;

            // Setup
            _mockITransactionService.Setup(x => x.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token)).ReturnsAsync(isSuccess);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.UpdateTransaction(
                    transactionId,
                    updateTransactionDto,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingUpdateTransactionMethod_ShouldReturnBadRequest_WhenInputTransactionIdIsInvalid(
            int transactionId)
        {
            // Arrange
            AddOrUpdateTransactionDto updateTransactionDto = new()
            {
                UserId = "TestUserId1",
                TransactionType = TransactionTypes.Debit,
                TransactionAmount = 1095,
            };
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.UpdateTransaction(
                    transactionId,
                    updateTransactionDto,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Fact]
        public async Task CallingDeleteTransactionMethod_ShouldReturnNoContent_WhenTransactionDeletedSuccessfully()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            const bool isSuccess = true;

            // Setup
            _mockITransactionService.Setup(x => x.DeleteTransaction(
                transactionId,
                tokenSource.Token)).ReturnsAsync(isSuccess);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.DeleteTransaction(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NoContentResult>();
                var noContentResult = (NoContentResult)actionResult;
                noContentResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.DeleteTransaction(
                    transactionId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingDeleteTransactionMethod_ShouldReturnNotFound_WhenTransactionDeleteIsFailed()
        {
            // Arrange
            const int transactionId = 101;
            CancellationTokenSource tokenSource = new();
            const bool isSuccess = false;

            // Setup
            _mockITransactionService.Setup(x => x.DeleteTransaction(
                transactionId,
                tokenSource.Token)).ReturnsAsync(isSuccess);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.DeleteTransaction(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.DeleteTransaction(
                    transactionId,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingDeleteTransactionMethod_ShouldReturnBadRequest_WhenInputTransactionIdIsInvalid(
            int transactionId)
        {
            // Arrange
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.DeleteTransaction(
                transactionId,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.DeleteTransaction(
                    transactionId,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Fact]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnTransactions_WhenInputValidationPassed_And_BackEndReturnData()
        {
            // Arrange
            const decimal thresholdAmount = 1000;
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionDto> transactions =
            [
                new()
                {
                    TransactionId = 101,
                    UserId = "TestUserId1",
                    TransactionType = TransactionTypes.Debit,
                    TransactionAmount =  1095,
                    TransactionCreatedAt = DateTime.UtcNow,
                }
            ];

            // Setup
            _mockITransactionService.Setup(x => x.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(transactions);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
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
                okObjectResult.Value.Should().BeSameAs(transactions);

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnNotFound_WhenInputValidationPassed_But_BackEndReturnNoData()
        {
            // Arrange
            const decimal thresholdAmount = 1000;
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionDto> transactions = [];

            // Setup
            _mockITransactionService.Setup(x => x.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token)).ReturnsAsync(transactions);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
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

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnBadRequest_WhenInputPageSizeExceedsMaxLimit()
        {
            // Arrange
            const decimal thresholdAmount = 1000;
            const int pageNumber = 2;
            const int pageSize = ApplicationConstants.TransactionMaxPageSize + 1;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetHighVolumeTransactionsMethod_ShouldReturnBadRequest_WhenInputPageSizeIsInvalid(
            int pageSize)
        {
            // Arrange
            const decimal thresholdAmount = 1000;
            const int pageNumber = 2;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetTransactionsGetHighVolumeTransactionsMethod_ShouldReturnBadRequest_WhenInputPageNumberIsInvalid(
           int pageNumber)
        {
            // Arrange
            const decimal thresholdAmount = 1000;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public async Task CallingGetTransactionsGetHighVolumeTransactionsMethod_ShouldReturnBadRequest_WhenInputThresholdAmountIsInvalid(
           decimal thresholdAmount)
        {
            // Arrange
            const int pageNumber = 2;
            const int pageSize = 10;
            CancellationTokenSource tokenSource = new();

            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<BadRequestObjectResult>();
                var badRequestObjectResult = (BadRequestObjectResult)actionResult;
                badRequestObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionService.Verify(x => x.GetHighVolumeTransactions(
                    thresholdAmount,
                    pageNumber,
                    pageSize,
                    tokenSource.Token),
                    Times.Never());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsByTransactionTypeMethod_ShouldReturnTransactions_WhenBackEndReturnData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos =
            [
                new()
                {
                    TransactionType = TransactionTypes.Debit,
                    TotalTransactionAmount = 15704
                },
                new()
                {
                    TransactionType = TransactionTypes.Credit,
                    TotalTransactionAmount = 18752
                }
            ];

            // Setup
            _mockITransactionSummaryService.Setup(x => x.GetTransactionsByTransactionType(
                tokenSource.Token))
                .ReturnsAsync(transactionByTransactionTypeDtos);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionsByTransactionType(
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<OkObjectResult>();
                var okObjectResult = (OkObjectResult)actionResult;
                okObjectResult.Should().NotBeNull();
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.Value.Should().BeSameAs(transactionByTransactionTypeDtos);

                // Verify
                _mockITransactionSummaryService.Verify(x => x.GetTransactionsByTransactionType(
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsByTransactionTypeMethod_ShouldReturnNotFound_WhenBackEndReturnNoData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = [];

            // Setup
            _mockITransactionSummaryService.Setup(x => x.GetTransactionsByTransactionType(
                tokenSource.Token))
                .ReturnsAsync(transactionByTransactionTypeDtos);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionsByTransactionType(
                 tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionSummaryService.Verify(x => x.GetTransactionsByTransactionType(
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsByUserMethod_ShouldReturnTransactions_WhenBackEndReturnData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionByUserDto> transactionByUserDtos =
            [
                new()
                {
                    UserId = "TestUser1",
                    TotalTransactionAmount = 15704
                },
                new()
                {
                    UserId = "TestUser2",
                    TotalTransactionAmount = 18752
                }
            ];

            // Setup
            _mockITransactionSummaryService.Setup(x => x.GetTransactionsByUser(
                tokenSource.Token))
                .ReturnsAsync(transactionByUserDtos);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionsByUser(
                tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<OkObjectResult>();
                var okObjectResult = (OkObjectResult)actionResult;
                okObjectResult.Should().NotBeNull();
                okObjectResult.Value.Should().NotBeNull();
                okObjectResult.Value.Should().BeSameAs(transactionByUserDtos);

                // Verify
                _mockITransactionSummaryService.Verify(x => x.GetTransactionsByUser(
                    tokenSource.Token),
                    Times.Once());
            }
        }

        [Fact]
        public async Task CallingGetTransactionsByUserMethod_ShouldReturnNotFound_WhenBackEndReturnNoData()
        {
            // Arrange
            CancellationTokenSource tokenSource = new();
            IEnumerable<TransactionByUserDto> transactionByUserDtos = [];

            // Setup
            _mockITransactionSummaryService.Setup(x => x.GetTransactionsByUser(
                tokenSource.Token))
                .ReturnsAsync(transactionByUserDtos);


            // Act
            IActionResult actionResult = await _transactionsControllerUnderTest.GetTransactionsByUser(
                 tokenSource.Token);

            // Assert
            using (new AssertionScope())
            {
                actionResult.Should().NotBeNull();
                actionResult.Should().BeOfType<NotFoundObjectResult>();
                var notFoundObjectResult = (NotFoundObjectResult)actionResult;
                notFoundObjectResult.Should().NotBeNull();

                // Verify
                _mockITransactionSummaryService.Verify(x => x.GetTransactionsByUser(
                    tokenSource.Token),
                    Times.Once());
            }
        }
    }
}
