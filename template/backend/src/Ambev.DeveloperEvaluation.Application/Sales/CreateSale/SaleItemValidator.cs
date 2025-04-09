using Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Validator for validating individual sale item commands.
    /// Ensures product name, quantity, and unit price follow business rules.
    /// </summary>
    public class SaleItemValidator : AbstractValidator<AddSaleItemCommand>
    {
        /// <summary>
        /// Initializes the validation rules for <see cref="AddSaleItemCommand"/>.
        /// </summary>
        public SaleItemValidator()
        {
            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required.")
                .MaximumLength(100).WithMessage("Product name must be 100 characters or fewer.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
