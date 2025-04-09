using Ambev.DeveloperEvaluation.Application.SaleItems.Dtos;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
/// Contains unit tests for the <see cref="UpdateSaleHandler"/> class.
/// </summary>
public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;
    private readonly UpdateSaleHandler _handler;

    /// <summary>
    /// Initializes test dependencies.
    /// </summary>
    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _eventPublisher);
    }

    /// <summary>
    /// Ensures the handler returns a valid result when updating a valid sale.
    /// </summary>
    [Fact(DisplayName = "Given valid sale data When updating sale Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        var sale = new Sale
        {
            Id = command.Id,
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

        var result = new UpdateSaleResult
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

        _mapper.Map<UpdateSaleResult>(sale).Returns(result);
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(sale);
        _saleRepository.UpdateAsync(sale, Arg.Any<CancellationToken>()).Returns(sale);

        // Act
        var updateResult = await _handler.Handle(command, CancellationToken.None);

        // Assert
        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(sale.Id);
        await _saleRepository.Received(1).UpdateAsync(sale, Arg.Any<CancellationToken>());
        await _eventPublisher.Received(1).PublishAsync(
            Arg.Is<SaleModifiedEvent>(e =>
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
    /// Ensures the handler throws a KeyNotFoundException when sale ID is invalid.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When updating Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentSale_ThrowsKeyNotFoundException()
    {
        // Arrange
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    /// <summary>
    /// Ensures the handler throws a validation exception when data is invalid.
    /// </summary>
    [Fact(DisplayName = "Given invalid update data When updating Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Arrange
        var command = new UpdateSaleCommand(); // Incomplete data

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ValidationException>();
    }
}
