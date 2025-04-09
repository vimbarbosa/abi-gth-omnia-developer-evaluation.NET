namespace Ambev.DeveloperEvaluation.Application.SaleItems.Dtos
{
    public class SaleItemDto
    {
        public Guid Id { get; set; }
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public bool IsCancelled { get; set; }
    }
}
