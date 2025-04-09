namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem
{
    /// <summary>
    /// Represents the result of adding an item to a sale.
    /// </summary>
    public class AddSaleItemResponse
    {
        /// <summary>
        /// Gets or sets the ID of the added item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount applied to this item.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the total amount (Quantity * UnitPrice - Discount).
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating whether this item was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
