namespace Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem
{
    /// <summary>
    /// Represents the response payload for an item added to a sale.
    /// </summary>
    public class AddSaleItemResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the associated sale ID.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product.
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
        /// Gets or sets the total price of the item (Quantity * UnitPrice - Discount).
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets whether the item is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
