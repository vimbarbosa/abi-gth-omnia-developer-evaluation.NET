using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; } = string.Empty;
        public DateTime SaleDate { get; set; }
        public string Customer { get; set; } = string.Empty;
        public string Branch { get; set; } = string.Empty;
        public string Product { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; } // Pode ser calculado: (Quantity * UnitPrice - Discount)
        public bool IsCancelled { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public static decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new ArgumentOutOfRangeException(nameof(quantity), "Cannot sell more than 20 identical items.");

            if (quantity >= 10)
                return unitPrice * quantity * 0.20m;
            if (quantity >= 4)
                return unitPrice * quantity * 0.10m;
            return 0;
        }
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
            };
        }
    }
}
