using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Provides validation rules for the <see cref="GetSaleRequest"/> class.
    /// Ensures that a valid sale ID is provided in the request.
    /// </summary>
    public class GetSaleRequestValidator : AbstractValidator<GetSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleRequestValidator"/> class.
        /// Applies validation rules for the sale ID.
        /// </summary>
        public GetSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("The sale ID is required.");
        }
    }
}
