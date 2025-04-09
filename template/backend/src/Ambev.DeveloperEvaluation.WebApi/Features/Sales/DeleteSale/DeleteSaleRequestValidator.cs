using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.DeleteSale
{
    /// <summary>
    /// Validates the <see cref="DeleteSaleRequest"/> object.
    /// Ensures that required fields are provided and valid.
    /// </summary>
    public class DeleteSaleRequestValidator : AbstractValidator<DeleteSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSaleRequestValidator"/> class.
        /// </summary>
        public DeleteSaleRequestValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("The sale ID is required.");
        }
    }
}
