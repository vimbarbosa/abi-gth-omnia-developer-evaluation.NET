using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    /// <summary>
    /// Provides methods for generating test data using the Bogus library.
    /// Centralizes creation of valid and realistic CreateSaleCommand objects.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
            .RuleFor(s => s.Customer, f => f.Name.FullName())
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.Product, f => f.Commerce.ProductName())
            .RuleFor(s => s.Quantity, f => f.Random.Int(1, 10))
            .RuleFor(s => s.UnitPrice, f => f.Random.Decimal(10, 500))
            .RuleFor(s => s.Discount, f => f.Random.Decimal(0, 50))
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .RuleFor(s => s.Total, (f, s) => (s.UnitPrice * s.Quantity) - s.Discount);

        /// <summary>
        /// Generates a valid CreateSaleCommand with randomized values.
        /// </summary>
        public static CreateSaleCommand GenerateValidCommand()
        {
            return createSaleHandlerFaker.Generate();
        }

        /// <summary>
        /// Generates a list of valid CreateSaleCommand objects.
        /// </summary>
        public static List<CreateSaleCommand> GenerateMultipleValidCommands(int count = 5)
        {
            return createSaleHandlerFaker.Generate(count);
        }
    }
}