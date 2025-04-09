using Ambev.DeveloperEvaluation.Application.SaleItems.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sales.TestData;
using AutoMapper;
using FluentAssertions;
using FluentValidation;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Contains unit tests for the <see cref="CreateSaleHandler"/> class.
/// </summary>
public class CreateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly CreateSaleHandler _handler;

    /// <summary>
    /// Initializes the test setup with mocked dependencies.
    /// </summary>
    public CreateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new CreateSaleHandler(_saleRepository, _mapper, _eventPublisher);
    }

    /// <summary>
    /// Validates that a valid sale command returns a successful result.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When creating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = Guid.NewGuid(),
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Customer = command.Customer,
            Branch = command.Branch,
            IsCancelled = command.IsCancelled,
            Items = command.Items.Select(i => new SaleItem
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };

        var result = new CreateSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            SaleDate = sale.SaleDate,
            Customer = sale.Customer,
            Branch = sale.Branch,
            IsCancelled = sale.IsCancelled,
            Items = sale.Items.Select(item => new SaleItemDto
            {
                Product = item.Product,
                Quantity = item.Quantity,
                UnitPrice = item.UnitPrice,
                Discount = item.Discount,
                Total = item.Total
            }).ToList()
        };

        var saleCreatedEvent = new SaleCreatedEvent { Payload = sale };

        _mapper.Map<Sale>(command).Returns(sale);
        _mapper.Map<CreateSaleResult>(sale).Returns(result);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var createSaleResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        createSaleResult.Should().NotBeNull();
        createSaleResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).CreateAsync(sale, Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishAsync(
            Arg.Is<SaleCreatedEvent>(e =>
                e.Payload.SaleNumber == sale.SaleNumber &&
                e.Payload.Customer == sale.Customer &&
                e.Payload.Branch == sale.Branch &&
                e.Payload.Items.Count == sale.Items.Count &&
                e.Payload.Items.All(i =>
                    sale.Items.Any(x =>
                        x.Product == i.Product &&
                        x.Quantity == i.Quantity &&
                        x.UnitPrice == i.UnitPrice))),
            "sales.events", Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Validates discount calculation for multiple quantity ranges.
    /// </summary>
    [Theory(DisplayName = "Given different quantity ranges When calculating discount Then applies correct discount")]
    [InlineData(2, 100, 0)]       // No discount
    [InlineData(4, 100, 40)]      // 10%
    [InlineData(10, 100, 200)]    // 20%
    [InlineData(20, 100, 400)]    // 20%
    public void CalculateDiscount_ValidQuantities_ReturnsExpectedDiscount(int quantity, decimal unitPrice, decimal expectedDiscount)
    {
        // Act
        var discount = SaleItem.CalculateDiscount(quantity, unitPrice);

        // Assert
        discount.Should().Be(expectedDiscount);
    }

    /// <summary>
    /// Ensures that invalid command triggers validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid sale data When creating sale Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new CreateSaleCommand(); // Invalid

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }

    /// <summary>
    /// Ensures AutoMapper is correctly mapping the command to the domain entity.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to sale entity")]
    public async Task Handle_ValidRequest_MapsCommandToSale()
    {
        // Arrange
        var command = CreateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale { Id = Guid.NewGuid() };

        _mapper.Map<Sale>(command).Returns(sale);
        _saleRepository.CreateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapper.Received(1).Map<Sale>(Arg.Is<CreateSaleCommand>(c => c.SaleNumber == command.SaleNumber));
    }
}
