using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem
{
    /// <summary>
    /// Validator for AddSaleItemCommand ensuring all required fields are valid.
    /// </summary>
    public class AddSaleItemValidator : AbstractValidator<AddSaleItemCommand>
    {
        public AddSaleItemValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.");

            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        }
    }
}
