using Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    /// <summary>
    /// Validates the fields of the <see cref="UpdateSaleRequest"/> to ensure all required data is present and valid.
    /// </summary>
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        /// <summary>
        /// Initializes the validation rules for updating a sale.
        /// </summary>
        public UpdateSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Sale ID is required.");

            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number must not exceed 50 characters.");

            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(x => x.Customer)
                .NotEmpty().WithMessage("Customer is required.")
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

            RuleFor(x => x.Items)
                .NotNull().WithMessage("Sale must include at least one item.")
                .Must(items => items.Count > 0).WithMessage("Sale must have at least one item.");

            RuleForEach(x => x.Items)
                .SetValidator(new AddSaleItemRequestValidator());
        }
    }
}
