using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents the event triggered when a sale is modified.
    /// </summary>
    public class SaleModifiedEvent
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// Default value is "SaleModified".
        /// </summary>
        public string Event { get; set; } = "SaleModified";

        /// <summary>
        /// Gets or sets the payload containing the modified sale data.
        /// </summary>
        public Sale Payload { get; set; } = default!;
    }
}
