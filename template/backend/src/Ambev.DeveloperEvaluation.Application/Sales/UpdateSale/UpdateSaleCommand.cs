using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Command to update an existing sale.
    /// </summary>
    public class UpdateSaleCommand : IRequest<UpdateSaleResult>
    {
        /// <summary>
        /// The unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The unique number identifying the sale.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// The date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// The name of the customer.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// The branch where the sale occurred.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// The product sold.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Price per unit of product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to the sale.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total value of the sale item (after applying discount).
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Whether the sale was cancelled.
        /// </summary>
        public bool Cancelled { get; set; }
    }
}
