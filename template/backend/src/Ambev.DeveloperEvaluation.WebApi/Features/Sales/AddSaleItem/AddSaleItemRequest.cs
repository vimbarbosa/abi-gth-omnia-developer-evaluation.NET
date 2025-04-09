namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem
{
    /// <summary>
    /// Represents the request payload to add a new item to an existing sale.
    /// </summary>
    public class AddSaleItemRequest
    {
        /// <summary>
        /// Gets or sets the product name of the item being sold.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the quantity of the product being sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
