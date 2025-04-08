namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents the response returned after retrieving a sale.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier for the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number (reference code).
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the customer who made the purchase.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the branch where the sale was made.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the product sold.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount applied to the item.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the total amount for the item after discount.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets the total sale amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the sale was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
