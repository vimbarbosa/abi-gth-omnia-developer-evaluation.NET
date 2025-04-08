using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
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

            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required.")
                .MaximumLength(100).WithMessage("Product name must be 100 characters or fewer.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        }
    }
}
