using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

/// <summary>
/// Provides reusable test data for the <see cref="Sale"/> entity.
/// Ensures valid and consistent sale objects with sale items for unit tests.
/// </summary>
public static class SaleTestData
{
    private static readonly Faker<SaleItem> validSaleItemFaker = new Faker<SaleItem>()
        .RuleFor(i => i.Product, f => f.Commerce.ProductName())
        .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(i => i.UnitPrice, f => f.Random.Decimal(10m, 500m))
        .RuleFor(i => i.IsCancelled, f => false)
        .FinishWith((f, i) =>
        {
            i.Discount = SaleItem.CalculateDiscount(i.Quantity, i.UnitPrice);
        });

    private static readonly Faker<Sale> validSaleFaker = new Faker<Sale>()
        .RuleFor(s => s.Id, f => Guid.NewGuid())
        .RuleFor(s => s.SaleNumber, f => $"SAL-{f.Random.Int(1000, 9999)}")
        .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
        .RuleFor(s => s.Customer, f => f.Name.FullName())
        .RuleFor(s => s.Branch, f => f.Company.CompanyName())
        .RuleFor(s => s.IsCancelled, f => false)
        .RuleFor(s => s.CreatedAt, f => DateTime.UtcNow)
        .RuleFor(s => s.UpdatedAt, f => null)
        .RuleFor(s => s.Items, f =>
        {
            var itemCount = f.Random.Int(1, 5);
            return validSaleItemFaker.Generate(itemCount);
        });

    /// <summary>
    /// Generates a valid Sale entity with items for positive test cases.
    /// </summary>
    public static Sale GenerateValidSale()
    {
        return validSaleFaker.Generate();
    }

    /// <summary>
    /// Generates a list of valid Sale entities with items.
    /// </summary>
    public static List<Sale> GenerateMultipleValidSales(int count = 3)
    {
        return validSaleFaker.Generate(count);
    }
}
