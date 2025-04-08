using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData
{
    public static class UpdateSaleHandlerTestData
    {
        private static readonly Faker<UpdateSaleCommand> updateSaleHandlerFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.SaleNumber, f => f.Random.Int(1000, 9999).ToString())
            .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
            .RuleFor(s => s.Customer, f => f.Name.FullName())
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.Product, f => f.Commerce.ProductName())
            .RuleFor(s => s.Quantity, f => f.Random.Int(1, 20))
            .RuleFor(s => s.UnitPrice, f => f.Finance.Amount(10, 1000))
            .RuleFor(s => s.Discount, f => 0) // será recalculado se necessário
            .RuleFor(s => s.Total, (f, s) => s.Quantity * s.UnitPrice)
            .RuleFor(s => s.Cancelled, f => f.Random.Bool());

        public static UpdateSaleCommand GenerateValidCommand()
        {
            var command = updateSaleHandlerFaker.Generate();
            return ApplyDiscountRules(command);
        }

        public static UpdateSaleCommand GenerateValidCommand(int quantity)
        {
            var command = updateSaleHandlerFaker.Generate();
            command.Quantity = quantity;
            return ApplyDiscountRules(command);
        }

        public static List<UpdateSaleCommand> GenerateMultipleValidCommands(int count = 5)
        {
            return Enumerable.Range(1, count)
                .Select(_ => GenerateValidCommand())
                .ToList();
        }

        private static UpdateSaleCommand ApplyDiscountRules(UpdateSaleCommand command)
        {
            if (command.Quantity >= 4 && command.Quantity < 10)
            {
                command.Discount = command.Quantity * command.UnitPrice * 0.10m;
            }
            else if (command.Quantity >= 10 && command.Quantity <= 20)
            {
                command.Discount = command.Quantity * command.UnitPrice * 0.20m;
            }
            else
            {
                command.Discount = 0;
            }

            command.Total = (command.Quantity * command.UnitPrice) - command.Discount;

            return command;
        }
    }
}