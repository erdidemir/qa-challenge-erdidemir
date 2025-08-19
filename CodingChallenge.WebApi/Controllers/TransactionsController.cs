using CodingChallenge.Common.Constants;
using CodingChallenge.Dtos;
using CodingChallenge.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CodingChallenge.WebApi.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    public class TransactionsController(
        ITransactionService transactionService,
        ITransactionSummaryService transactionSummaryService) : ControllerBase
    {
        /// <summary>
        /// Get the transactions.
        /// </summary>
        /// <param name="pageNumber">The page number value.</param>
        /// <param name="pageSize">The page size value.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>A list of transactions <seealso cref="TransactionDto"/>.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTransactions(
            [FromQuery]int pageNumber = ApplicationConstants.TransactionDefaultPageNumber,
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

            IEnumerable<TransactionDto> transactions = await transactionService.GetTransactions(
                pageNumber,
                pageSize,
                cancellationToken);

            if (transactions is null || !transactions.Any())
            {
                return NotFound(SharedResources.TransactionsNotFoundErrorMessage);
            }

            return Ok(transactions);
        }

        /// <summary>
        /// Get a transaction by ID.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The requested transaction <seealso cref="TransactionDto"/>.</returns>
        [HttpGet("{transactionId}")]
        [ProducesResponseType(typeof(TransactionDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTransactionById(
            [FromRoute] int transactionId,
            CancellationToken cancellationToken = default)
        {
            if (transactionId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            TransactionDto? transaction = await transactionService.GetTransactionById(
                transactionId,
                cancellationToken);

            if (transaction is null)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, transactionId));
            }

            return Ok(transaction);
        }

        /// <summary>
        /// Add a Transaction.
        /// </summary>
        /// <param name="addTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The Id value of added transaction.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> AddTransaction(
            [FromBody] AddOrUpdateTransactionDto addTransactionDto,
            CancellationToken cancellationToken = default)
        {
            int transactionId = await transactionService.AddTransaction(
                addTransactionDto,
                cancellationToken);

            return Created($"/api/v1/Transactions/{transactionId}", transactionId);
        }

        /// <summary>
        /// Update the existing Transaction By Id.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="updateTransactionDto">The transaction <seealso cref="AddOrUpdateTransactionDto"/> object.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        [HttpPut("{transactionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> UpdateTransaction(
            [FromRoute] int transactionId,
            [FromBody] AddOrUpdateTransactionDto updateTransactionDto,
            CancellationToken cancellationToken = default)
        {
            if (transactionId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            bool isSuccess = await transactionService.UpdateTransaction(
                transactionId,
                updateTransactionDto,
                cancellationToken);

            if (!isSuccess)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, transactionId));
            }

            return NoContent();
        }

        /// <summary>
        /// Delete the existing Transaction By Id.
        /// </summary>
        /// <param name="transactionId">The ID of the transaction.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The success or failure for requested operation.</returns>
        [HttpDelete("{transactionId}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteTransaction(
           [FromRoute] int transactionId,
           CancellationToken cancellationToken = default)
        {
            if (transactionId <= 0)
            {
                return BadRequest(SharedResources.InvalidTransactionIdErrorMessage);
            }

            bool isSuccess = await transactionService.DeleteTransaction(
                transactionId,
                cancellationToken);

            if (!isSuccess)
            {
                return NotFound(string.Format(SharedResources.TransactionNotFoundErrorMessage, transactionId));
            }

            return NoContent();
        }

        /// <summary>
        /// Get the high volume transactions.
        /// </summary>
        /// <param name="thresholdAmount">The threshold amount value.</param>
        /// <param name="pageNumber">The page number value.</param>
        /// <param name="pageSize">The page size value.</param>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The list of high volumne transactions <seealso cref="TransactionDto"/>.</returns>
        [HttpGet("HighVolumeTransactions/{thresholdAmount}")]
        [ProducesResponseType(typeof(IEnumerable<TransactionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetHighVolumeTransactions(
            [FromRoute] decimal thresholdAmount,
            [FromQuery] int pageNumber = ApplicationConstants.TransactionDefaultPageNumber,
            [FromQuery] int pageSize = ApplicationConstants.TransactionDefaultPageSize,
            CancellationToken cancellationToken = default)
        {
            if (thresholdAmount <= 0)
            {
                return BadRequest(SharedResources.InvalidThresholdAmountErrorMessage);
            }

            if (pageNumber <= 0)
            {
                return BadRequest(SharedResources.InvalidPageNumberErrorMessage);
            }

            if (pageSize <= 0 || pageSize > ApplicationConstants.TransactionMaxPageSize)
            {
                return BadRequest(SharedResources.InvalidPageSizeErrorMessage);
            }

            IEnumerable<TransactionDto> transactions = await transactionService.GetHighVolumeTransactions(
                thresholdAmount,
                pageNumber,
                pageSize,
                cancellationToken);

            if (transactions is null || !transactions.Any())
            {
                return NotFound(SharedResources.TransactionsNotFoundErrorMessage);
            }

            return Ok(transactions);
        }

        /// <summary>
        /// Get the transactions by transaction type.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The list of transactions by transaction type <seealso cref="TransactionDto"/>.</returns>
        [HttpGet("GroupByTransactionType")]
        [ProducesResponseType(typeof(IEnumerable<TransactionByTransactionTypeDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTransactionsByTransactionType(
            CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionByTransactionTypeDto> transactionByTransactionTypeDtos = await transactionSummaryService.GetTransactionsByTransactionType(
               cancellationToken);

            if (transactionByTransactionTypeDtos is null || !transactionByTransactionTypeDtos.Any())
            {
                return NotFound(SharedResources.TransactionsNotFoundErrorMessage);
            }

            return Ok(transactionByTransactionTypeDtos);
        }

        /// <summary>
        /// Get the transactions by user id.
        /// </summary>
        /// <param name="cancellationToken">The cancellationToken.</param>
        /// <returns>The list of transactions by user id <seealso cref="TransactionDto"/>.</returns>
        [HttpGet("GroupByUser")]
        [ProducesResponseType(typeof(IEnumerable<TransactionByUserDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> GetTransactionsByUser(
           CancellationToken cancellationToken = default)
        {
            IEnumerable<TransactionByUserDto> transactionByUserDtos = await transactionSummaryService.GetTransactionsByUser(
               cancellationToken);

            if (transactionByUserDtos is null || !transactionByUserDtos.Any())
            {
                return NotFound(SharedResources.TransactionsNotFoundErrorMessage);
            }

            return Ok(transactionByUserDtos);
        }
    }
}
