using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Represents a command to delete a sale by its unique identifier.
    /// </summary>
    public class DeleteSaleCommand : IRequest<Unit>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to be deleted.
        /// </summary>
        public Guid Id { get; set; }
    }
}
