using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides reusable test data for the <see cref="Sale"/> entity.
/// Ensures valid and invalid sale objects for unit tests.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker<Sale> validSaleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
        .RuleFor(s => s.Customer, f => f.Name.FullName())
        .RuleFor(s => s.Branch, f => f.Company.CompanyName())
        .RuleFor(s => s.Product, f => f.Commerce.ProductName())
        .RuleFor(s => s.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(10, 500))
        .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
        .RuleFor(s => s.Discount, (f, s) =>
        {
            if (s.Quantity < 4) return 0;
            if (s.Quantity < 10) return s.UnitPrice * s.Quantity * 0.10m;
            return s.UnitPrice * s.Quantity * 0.20m;
        })
        .RuleFor(s => s.Total, (f, s) => s.UnitPrice * s.Quantity - s.Discount);

    /// <summary>
    /// Generates a valid Sale entity for positive test cases.
    /// </summary>
    public static Sale GenerateValidSale()
    {
        return validSaleFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Sale entities.
    /// </summary>
    public static List<Sale> GenerateMultipleValidSales(int count = 3)
    {
        return validSaleFaker.Generate(count);
    }

    /// <summary>
    /// Generates a Sale with a specific quantity (to test discount tiers).
    /// </summary>
    public static Sale GenerateValidSaleWithQuantity(int quantity)
    {
        var sale = validSaleFaker.Generate();
        sale.Quantity = quantity;
        sale.Discount = quantity < 4 ? 0 : quantity < 10 ? sale.UnitPrice * quantity * 0.10m : sale.UnitPrice * quantity * 0.20m;
        sale.Total = sale.UnitPrice * quantity - sale.Discount;
        return sale;
    }
}