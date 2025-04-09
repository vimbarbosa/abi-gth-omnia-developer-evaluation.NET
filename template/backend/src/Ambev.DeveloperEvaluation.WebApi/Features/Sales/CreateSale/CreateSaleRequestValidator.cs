using Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    /// <summary>
    /// Validator for <see cref="CreateSaleRequest"/>.
    /// Ensures that all required fields for creating a sale are present and valid.
    /// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleRequestValidator"/> class.
        /// </summary>
        public CreateSaleRequestValidator()
        {
            // Sale number validation
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number must not exceed 50 characters.");

            // Sale date validation
            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            // Customer name validation
            RuleFor(x => x.Customer)
                .NotEmpty().WithMessage("Customer is required.")
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

            // Branch name validation
            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

            // Items list validation
            RuleFor(x => x.Items)
                .NotNull().WithMessage("Sale must include at least one item.")
                .Must(items => items.Count > 0).WithMessage("Sale must have at least one item.");

            // Each item must be valid according to AddSaleItemRequestValidator
            RuleForEach(x => x.Items)
                .SetValidator(new AddSaleItemRequestValidator());
        }
    }
}
