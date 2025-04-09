using Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    /// <summary>
    /// Provides reusable test data for the <see cref="CreateSaleHandler"/> unit tests.
    /// Uses Bogus to generate valid and realistic data for test scenarios.
    /// </summary>
    public static class CreateSaleHandlerTestData
    {
        /// <summary>
        /// Configures a Faker to generate realistic and valid <see cref="CreateSaleCommand"/> instances.
        /// Includes randomized values for sale number, customer, branch, and item details.
        /// </summary>
        private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10))
            .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
            .RuleFor(s => s.Customer, f => f.Name.FullName())
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .RuleFor(s => s.Items, f => new List<AddSaleItemCommand>
            {
                new AddSaleItemCommand
                {
                    Product = f.Commerce.ProductName(),
                    Quantity = f.Random.Int(1, 20),
                    UnitPrice = f.Finance.Amount(10, 500),
                    IsCancelled = false
                }
            });

        /// <summary>
        /// Generates a single valid <see cref="CreateSaleCommand"/> instance for positive test scenarios.
        /// </summary>
        public static CreateSaleCommand GenerateValidCommand()
        {
            return createSaleHandlerFaker.Generate();
        }

        /// <summary>
        /// Generates a list of valid <see cref="CreateSaleCommand"/> instances.
        /// Useful for testing batch operations or collection validation.
        /// </summary>
        /// <param name="count">The number of commands to generate.</param>
        public static List<CreateSaleCommand> GenerateMultipleValidCommands(int count = 5)
        {
            return createSaleHandlerFaker.Generate(count);
        }
    }
}
