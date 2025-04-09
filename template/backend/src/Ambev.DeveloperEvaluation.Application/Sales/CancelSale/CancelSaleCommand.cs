using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Represents the command to cancel an existing sale.
    /// </summary>
    public class CancelSaleCommand : IRequest<Unit>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to be cancelled.
        /// </summary>
        public Guid SaleId { get; set; }
    }
}
