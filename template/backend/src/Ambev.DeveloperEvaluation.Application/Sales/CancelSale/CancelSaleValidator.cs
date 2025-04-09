using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Validator for the <see cref="CancelSaleCommand"/>.
    /// Ensures that the sale ID is provided and valid before attempting cancellation.
    /// </summary>
    public class CancelSaleValidator : AbstractValidator<CancelSaleCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaleValidator"/> class.
        /// Defines the validation rules for cancelling a sale.
        /// </summary>
        public CancelSaleValidator()
        {
            RuleFor(x => x.SaleId)
                .NotEmpty().WithMessage("Sale ID is required.");
        }
    }
}
