using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Validates the <see cref="GetSaleResult"/> object to ensure required fields are present and valid.
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleResult>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleValidator"/> class.
        /// Defines validation rules for retrieving a sale.
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
