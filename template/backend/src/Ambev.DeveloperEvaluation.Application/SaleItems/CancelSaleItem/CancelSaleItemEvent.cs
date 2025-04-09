using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CancelSaleItem
{
    public class CancelSaleItemEvent
    {
        public string Event { get; set; } = "ItemCancelled";
        public SaleItem Payload { get; set; } = default!;
    }
}
