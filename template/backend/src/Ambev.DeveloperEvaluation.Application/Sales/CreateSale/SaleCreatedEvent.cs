using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents an event triggered after a sale is successfully created.
    /// This event is used to notify other systems or services via message bus.
    /// </summary>
    public class SaleCreatedEvent
    {
        /// <summary>
        /// Gets or sets the name of the event.
        /// Default value is "SaleCreated".
        /// </summary>
        public string Event { get; set; } = "SaleCreated";

        /// <summary>
        /// Gets or sets the sale data payload associated with this event.
        /// Contains all the details of the created sale.
        /// </summary>
        public Sale Payload { get; set; } = default!;
    }
}
