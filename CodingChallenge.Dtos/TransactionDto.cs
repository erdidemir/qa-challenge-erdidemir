namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The Transaction Dto.
    /// </summary>
    public class TransactionDto : AddOrUpdateTransactionDto
    {
        /// <summary>
        /// Gets or sets the transaction id.
        /// </summary>
        /// <value>The transaction id value.</value>
        public required int TransactionId { get; set; }

        /// <summary>
        /// Gets or sets the transaction created at.
        /// </summary>
        /// <value>The transaction created at value.</value>
        public required DateTime TransactionCreatedAt { get; set; }
    }
}
