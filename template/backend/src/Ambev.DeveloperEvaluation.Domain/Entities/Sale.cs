using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale transaction that can contain multiple items.
    /// </summary>
    public class Sale
    {
        /// <summary>
        /// Unique identifier for the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique sale number identifying the transaction.
        /// </summary>
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// Name of the customer who made the purchase.
        /// </summary>
        public string Customer { get; set; } = string.Empty;

        /// <summary>
        /// Branch or store location where the sale occurred.
        /// </summary>
        public string Branch { get; set; } = string.Empty;

        /// <summary>
        /// Indicates whether the entire sale was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// List of items included in the sale.
        /// </summary>
        public List<SaleItem> Items { get; set; } = new();

        /// <summary>
        /// The total amount of the sale, excluding cancelled items.
        /// </summary>
        public decimal Total => Items
            .Where(i => !i.IsCancelled)
            .Sum(i => i.Total);

        /// <summary>
        /// Timestamp indicating when the sale was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Timestamp indicating when the sale was last updated, if applicable.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Validates the current sale instance using the domain validator.
        /// </summary>
        /// <returns>A detailed validation result including errors if any.</returns>
        public ValidationResultDetail Validate()
        {
            var validator = new SaleValidator();
            var result = validator.Validate(this);

            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(e => (ValidationErrorDetail)e)
            };
        }
    }
}
