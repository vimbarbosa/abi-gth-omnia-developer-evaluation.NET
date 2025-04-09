using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents a query to retrieve a sale by its unique identifier.
    /// </summary>
    public class GetSaleCommand : IRequest<GetSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to retrieve.
        /// </summary>
        public Guid Id { get; set; }
    }
}
