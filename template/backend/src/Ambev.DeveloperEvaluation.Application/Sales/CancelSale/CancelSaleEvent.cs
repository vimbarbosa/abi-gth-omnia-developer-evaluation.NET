using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Events
{
    /// <summary>
    /// Represents the event triggered when a sale is cancelled.
    /// </summary>
    public class CancelSaleEvent
    {
        /// <summary>
        /// Gets or sets the event name.
        /// Defaults to "SaleCancelled".
        /// </summary>
        public string Event { get; set; } = "SaleCancelled";

        /// <summary>
        /// Gets or sets the sale data associated with the cancellation event.
        /// </summary>
        public Sale Payload { get; set; } = default!;
    }
}
