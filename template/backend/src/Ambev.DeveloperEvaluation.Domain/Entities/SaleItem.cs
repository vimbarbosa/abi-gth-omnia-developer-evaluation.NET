namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents an individual item within a sale.
    /// Each item contains product details, pricing, and discount rules.
    /// </summary>
    public class SaleItem
    {
        /// <summary>
        /// Unique identifier for the sale item.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Foreign key referencing the associated sale.
        /// </summary>
        public Guid SaleId { get; set; }

        /// <summary>
        /// Name of the product being sold.
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied based on quantity.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total amount for the item after discount.
        /// Calculated as (Quantity * UnitPrice) - Discount.
        /// </summary>
        public decimal Total => (Quantity * UnitPrice) - Discount;

        /// <summary>
        /// Indicates whether the item was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Navigation property for the associated sale.
        /// </summary>
        public Sale Sale { get; set; } = default!;

        /// <summary>
        /// Calculates the discount based on the quantity of items purchased.
        /// - 4 to 9 items: 10% discount
        /// - 10 to 20 items: 20% discount
        /// - Over 20 items: not allowed
        /// </summary>
        /// <param name="quantity">The number of items purchased.</param>
        /// <param name="unitPrice">The price per unit.</param>
        /// <returns>The discount amount.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when quantity exceeds 20.</exception>
        public static decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Cannot sell more than 20 identical items.");

            if (quantity >= 10)
                return quantity * unitPrice * 0.20m;

            if (quantity >= 4)
                return quantity * unitPrice * 0.10m;

            return 0;
        }
    }
}
