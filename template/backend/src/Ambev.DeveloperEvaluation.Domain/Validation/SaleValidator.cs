using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Domain.Validation;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator()
    {
        RuleFor(s => s.SaleNumber)
            .NotEmpty().WithMessage("Sale number is required.")
            .MaximumLength(20).WithMessage("Sale number must not exceed 20 characters.");

        RuleFor(s => s.SaleDate)
            .NotEmpty().WithMessage("Sale date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Sale date cannot be in the future.");

        RuleFor(s => s.Customer)
            .NotEmpty().WithMessage("Customer is required.")
            .MaximumLength(100).WithMessage("Customer name must not exceed 100 characters.");

        RuleFor(s => s.Branch)
            .NotEmpty().WithMessage("Branch is required.")
            .MaximumLength(50).WithMessage("Branch must not exceed 50 characters.");

        RuleFor(s => s.Product)
            .NotEmpty().WithMessage("Product is required.")
            .MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

        RuleFor(s => s.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(s => s.UnitPrice)
            .GreaterThan(0).WithMessage("Unit price must be greater than zero.");

        RuleFor(s => s.Discount)
            .GreaterThanOrEqualTo(0).WithMessage("Discount must be zero or positive.")
            .LessThanOrEqualTo(s => s.Quantity * s.UnitPrice)
            .WithMessage("Discount cannot exceed total item price.");

        RuleFor(s => s.Total)
            .Equal(s => (s.Quantity * s.UnitPrice) - s.Discount)
            .WithMessage("Total must be equal to (Quantity * UnitPrice - Discount).");
    }
}