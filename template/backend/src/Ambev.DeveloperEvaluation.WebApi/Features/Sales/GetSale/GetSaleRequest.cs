namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Represents the request to retrieve a specific sale by its ID.
    /// </summary>
    public class GetSaleRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to be retrieved.
        /// </summary>
        public Guid Id { get; set; }
    }
}
