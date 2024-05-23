namespace DepositCalculator.Shared.Models
{
    /// <summary>
    /// Represents model with data for pagination.
    /// </summary>
    public class PaginationRequestModel
    {
        /// <summary>
        /// Gets or sets elements count per page.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or sets the specific page of items to retrieve.
        /// </summary>
        public int PageNumber { get; set; }
    }
}