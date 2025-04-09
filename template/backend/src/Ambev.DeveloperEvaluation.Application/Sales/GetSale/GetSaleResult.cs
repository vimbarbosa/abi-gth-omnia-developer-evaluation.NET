using Ambev.DeveloperEvaluation.Application.SaleItems.Dtos;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents the result data returned when retrieving a sale.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number used for tracking and reference.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date on which the sale occurred.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer who made the purchase.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the branch where the sale took place.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the sale has been cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the list of items included in the sale.
        /// </summary>
        public List<SaleItemDto> Items { get; set; } = new();

        /// <summary>
        /// Gets the total amount for the sale, calculated as the sum of non-cancelled item totals.
        /// </summary>
        public decimal TotalAmount => Items.Where(i => !i.IsCancelled).Sum(i => i.Total);
    }
}
