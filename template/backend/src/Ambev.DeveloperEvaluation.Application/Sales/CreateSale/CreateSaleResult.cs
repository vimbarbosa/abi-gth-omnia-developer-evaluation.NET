namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents the result of a successfully created sale.
    /// </summary>
    public class CreateSaleResult
    {
        /// <summary>
        /// Gets or sets the ID of the created sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Gets or sets the customer name.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the branch name.
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
        /// Gets or sets the unit price.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount applied.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the total amount for the item.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Gets or sets whether the sale was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
