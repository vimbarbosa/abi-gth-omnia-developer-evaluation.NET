using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Validator for the <see cref="UpdateSaleCommand"/> class.
    /// </summary>
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(s => s.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .MaximumLength(50).WithMessage("Sale number must not exceed 50 characters.");

            RuleFor(s => s.SaleDate)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

            RuleFor(s => s.Customer)
                .NotEmpty().WithMessage("Customer is required.")
                .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

            RuleFor(s => s.Branch)
                .NotEmpty().WithMessage("Branch is required.")
                .MaximumLength(100).WithMessage("Branch name must not exceed 100 characters.");

            RuleFor(s => s.Product)
                .NotEmpty().WithMessage("Product is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(s => s.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(s => s.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
