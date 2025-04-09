namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// Represents the request payload to delete an existing sale.
    /// </summary>
    public class DeleteSaleRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale to be deleted.
        /// </summary>
        public Guid Id { get; set; }
    }
}
