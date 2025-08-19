namespace CodingChallenge.Dtos
{
    /// <summary>
    /// The Transaction By User Dto
    /// </summary>
    public class TransactionByUserDto
    {
        /// <summary>
        /// Gets or sets the user id.
        /// </summary>
        /// <value>The user id value.</value>
        public required string UserId { get; set; }

        /// <summary>
        /// Gets or sets the total transaction amount.
        /// </summary>
        /// <value>The total transaction amount value.</value>
        public required decimal TotalTransactionAmount { get; set; }
    }
}
