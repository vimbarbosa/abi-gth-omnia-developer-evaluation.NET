using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem
{
    /// <summary>
    /// Validator for the <see cref="AddSaleItemRequest"/> class.
    /// Ensures that item details meet business and data integrity rules.
    /// </summary>
    public class AddSaleItemRequestValidator : AbstractValidator<AddSaleItemRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddSaleItemRequestValidator"/> class.
        /// Defines validation rules for adding a sale item.
        /// </summary>
        public AddSaleItemRequestValidator()
        {
            RuleFor(x => x.Product)
                .NotEmpty().WithMessage("Product is required.")
                .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                .LessThanOrEqualTo(20).WithMessage("Cannot sell more than 20 identical items.");

            RuleFor(x => x.UnitPrice)
                .GreaterThan(0).WithMessage("Unit price must be greater than zero.");
        }
    }
}
