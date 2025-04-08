namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Result returned after successfully updating a sale.
    /// </summary>
    public class UpdateSaleResult
    {
        /// <summary>
        /// Unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique sale number.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date when the sale occurred.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Customer associated with the sale.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Branch where the sale was made.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Name of the product sold.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of products sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to the sale.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total amount for the sale item.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Indicates whether the sale was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
