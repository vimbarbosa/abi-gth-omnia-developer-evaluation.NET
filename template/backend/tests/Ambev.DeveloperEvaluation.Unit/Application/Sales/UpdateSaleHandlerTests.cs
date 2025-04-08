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

public class UpdateSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly UpdateSaleHandler _handler;
    private readonly IEventPublisher _eventPublisher;

    public UpdateSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _eventPublisher = Substitute.For<IEventPublisher>();
        _handler = new UpdateSaleHandler(_saleRepository, _mapper, _eventPublisher);
    }

    [Fact(DisplayName = "Given valid sale update data When updating Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(quantity: 5); // 10% discount

        var expectedDiscount = command.UnitPrice * command.Quantity * 0.10m;
        var expectedTotal = (command.UnitPrice * command.Quantity) - expectedDiscount;

        var existingSale = new Sale
        {
            Id = command.Id,
            Product = command.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = expectedDiscount,
            Total = expectedTotal,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Customer = command.Customer,
            SaleNumber = command.SaleNumber,
            IsCancelled = command.Cancelled
        };

        var result = new UpdateSaleResult
        {
            Id = command.Id,
            SaleNumber = command.SaleNumber,
            SaleDate = command.SaleDate,
            Customer = command.Customer,
            Branch = command.Branch,
            Product = command.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = expectedDiscount,
            Total = expectedTotal,
            IsCancelled = command.Cancelled
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map(command, existingSale).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(result);

        var updateResult = await _handler.Handle(command, CancellationToken.None);

        updateResult.Should().NotBeNull();
        updateResult.Id.Should().Be(command.Id);
        updateResult.Total.Should().Be(expectedTotal);
        updateResult.Discount.Should().Be(expectedDiscount);
    }

    [Fact(DisplayName = "Given quantity less than 4 When updating sale Then discount should be zero")]
    public async Task Handle_QuantityLessThan4_DiscountShouldBeZero()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(quantity: 3);
        var expectedTotal = command.UnitPrice * command.Quantity;

        var existingSale = new Sale
        {
            Id = command.Id,
            Product = command.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = 0m,
            Total = expectedTotal,
            SaleDate = command.SaleDate,
            Branch = command.Branch,
            Customer = command.Customer,
            SaleNumber = command.SaleNumber,
            IsCancelled = command.Cancelled
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map(command, existingSale).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(new UpdateSaleResult
        {
            Id = existingSale.Id,
            Product = existingSale.Product,
            Quantity = existingSale.Quantity,
            UnitPrice = existingSale.UnitPrice,
            Discount = existingSale.Discount,
            Total = existingSale.Total,
            Branch = existingSale.Branch,
            Customer = existingSale.Customer,
            SaleNumber = existingSale.SaleNumber,
            SaleDate = existingSale.SaleDate,
            IsCancelled = existingSale.IsCancelled
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Discount.Should().Be(0m);
        result.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Given quantity between 10 and 20 When updating Then applies 20% discount")]
    public async Task Handle_QuantityBetween10And20_AppliesTwentyPercentDiscount()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(quantity: 10);
        var expectedDiscount = command.UnitPrice * command.Quantity * 0.20m;
        var expectedTotal = (command.UnitPrice * command.Quantity) - expectedDiscount;

        var existingSale = new Sale
        {
            Id = command.Id,
            Product = command.Product,
            Quantity = command.Quantity,
            UnitPrice = command.UnitPrice,
            Discount = expectedDiscount,
            Total = expectedTotal,
            Branch = command.Branch,
            Customer = command.Customer,
            SaleDate = command.SaleDate,
            SaleNumber = command.SaleNumber,
            IsCancelled = command.Cancelled
        };

        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map(command, existingSale).Returns(existingSale);
        _saleRepository.UpdateAsync(existingSale, Arg.Any<CancellationToken>()).Returns(existingSale);
        _mapper.Map<UpdateSaleResult>(existingSale).Returns(new UpdateSaleResult
        {
            Id = existingSale.Id,
            Product = existingSale.Product,
            Quantity = existingSale.Quantity,
            UnitPrice = existingSale.UnitPrice,
            Discount = existingSale.Discount,
            Total = existingSale.Total,
            Branch = existingSale.Branch,
            Customer = existingSale.Customer,
            SaleNumber = existingSale.SaleNumber,
            SaleDate = existingSale.SaleDate,
            IsCancelled = existingSale.IsCancelled
        });

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Discount.Should().Be(expectedDiscount);
        result.Total.Should().Be(expectedTotal);
    }

    [Fact(DisplayName = "Given quantity greater than 20 When updating Then throws validation exception")]
    public async Task Handle_QuantityGreaterThan20_ThrowsValidationException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand(quantity: 25);
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Cannot sell more than 20 identical items*");
    }

    [Fact(DisplayName = "Given non-existent sale ID When updating Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentId_ThrowsException()
    {
        var command = UpdateSaleHandlerTestData.GenerateValidCommand();
        _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>()).Returns((Sale?)null);
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {command.Id} not found");
    }

    [Fact(DisplayName = "Given invalid data When updating Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        var command = new UpdateSaleCommand(); // inválido
        var act = () => _handler.Handle(command, CancellationToken.None);
        await act.Should().ThrowAsync<ValidationException>();
    }
}