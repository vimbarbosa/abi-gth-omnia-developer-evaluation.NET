using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for the CreateSaleCommand.
    /// Ensures that all required fields for a sale are provided and valid.
    /// </summary>
    public class CreateSaleValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleValidator()
        {
            RuleFor(x => x.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(20).WithMessage("Sale number must be 20 characters or fewer.");

            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(x => x.Customer)
                .NotEmpty().WithMessage("Customer is required.")
                .MaximumLength(100).WithMessage("Customer name must be 100 characters or fewer.");

            RuleFor(x => x.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(100).WithMessage("Branch name must be 100 characters or fewer.");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one sale item is required.");

            RuleForEach(x => x.Items).SetValidator(new SaleItemValidator());
        }
    }
}
