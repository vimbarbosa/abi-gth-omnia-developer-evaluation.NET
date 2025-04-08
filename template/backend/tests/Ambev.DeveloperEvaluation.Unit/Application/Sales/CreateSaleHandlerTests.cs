using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using OneOf.Types;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    /// <summary>
    /// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
    /// </summary>
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;
        private readonly CreateSaleHandler _handler;

        public CreateSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _eventPublisher = Substitute.For<IEventPublisher>();
            _handler = new CreateSaleHandler(_saleRepository, _mapper, _eventPublisher);
        }

        [Theory(DisplayName = "Given valid quantity When creating sale Then calculates correct discount and total")]
        [InlineData(3, 100, 0, 300)]     // No discount
        [InlineData(4, 100, 40, 360)]    // 10%
        [InlineData(10, 100, 200, 800)]  // 20%
        [InlineData(20, 100, 400, 1600)] // 20%
        public async Task Handle_ValidQuantities_ShouldCalculateCorrectDiscountAndTotal(int quantity, decimal unitPrice, decimal expectedDiscount, decimal expectedTotal)
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "SAL123456",
                SaleDate = DateTime.UtcNow,
                Customer = "Cliente Teste",
                Branch = "Filial A",
                Product = "Produto X",
                Quantity = quantity,
                UnitPrice = unitPrice,
                IsCancelled = false
            };

            var expectedSale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = command.SaleNumber,
                SaleDate = command.SaleDate,
                Customer = command.Customer,
                Branch = command.Branch,
                Product = command.Product,
                Quantity = command.Quantity,
                UnitPrice = command.UnitPrice,
                Discount = expectedDiscount,
                Total = expectedTotal,
                IsCancelled = command.IsCancelled
            };

            var expectedResult = new CreateSaleResult { Id = expectedSale.Id };

            _mapper.Map<Sale>(command).Returns(expectedSale);
            _mapper.Map<CreateSaleResult>(expectedSale).Returns(expectedResult);
            _saleRepository.CreateAsync(expectedSale, Arg.Any<CancellationToken>()).Returns(expectedSale);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedSale.Id);

            await _saleRepository.Received(1).CreateAsync(expectedSale, Arg.Any<CancellationToken>());
            await _eventPublisher.Received(1).PublishAsync(
                Arg.Is<SaleCreatedEvent>(e =>
                    e.Event == "SaleCreated" &&
                    e.Payload.Id == expectedResult.Id &&
                    e.Payload.SaleNumber == expectedResult.SaleNumber &&
                    e.Payload.Total == expectedResult.Total
                 ), "sales.events", Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var command = new CreateSaleCommand(); // Missing required fields

            // Act
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ValidationException>();
        }
    }
}
