namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class SaleCreatedEvent
    {
        public string Event { get; set; } = "SaleCreated";
        public CreateSaleResult Payload { get; set; } = default!;
    }
}
