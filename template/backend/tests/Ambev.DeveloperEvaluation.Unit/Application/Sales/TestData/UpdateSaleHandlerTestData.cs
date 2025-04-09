using Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    /// <summary>
    /// Provides reusable test data for <see cref="UpdateSaleCommand"/> using Bogus.
    /// Ensures generation of valid data for testing update sale scenarios.
    /// </summary>
    public static class UpdateSaleHandlerTestData
    {
        /// <summary>
        /// Faker instance configured to generate realistic and valid update sale commands.
        /// </summary>
        private static readonly Faker<UpdateSaleCommand> updateSaleHandlerFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.SaleNumber, f => f.Random.AlphaNumeric(10).ToUpper())
            .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
            .RuleFor(s => s.Customer, f => f.Name.FullName())
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.IsCancelled, f => f.Random.Bool())
            .RuleFor(s => s.Items, f =>
            {
                var itemFaker = new Faker<AddSaleItemCommand>()
                    .RuleFor(i => i.Product, f => f.Commerce.ProductName())
                    .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
                    .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 1000));

                return itemFaker.Generate(f.Random.Int(1, 3));
            });

        /// <summary>
        /// Generates a single valid <see cref="UpdateSaleCommand"/> instance.
        /// </summary>
        /// <returns>A valid <see cref="UpdateSaleCommand"/> for unit testing.</returns>
        public static UpdateSaleCommand GenerateValidCommand()
        {
            return updateSaleHandlerFaker.Generate();
        }

        /// <summary>
        /// Generates a list of valid <see cref="UpdateSaleCommand"/> instances.
        /// </summary>
        /// <param name="count">The number of commands to generate.</param>
        /// <returns>A list of valid <see cref="UpdateSaleCommand"/> for unit testing.</returns>
        public static List<UpdateSaleCommand> GenerateMultipleValidCommands(int count = 5)
        {
            return updateSaleHandlerFaker.Generate(count);
        }
    }
}
