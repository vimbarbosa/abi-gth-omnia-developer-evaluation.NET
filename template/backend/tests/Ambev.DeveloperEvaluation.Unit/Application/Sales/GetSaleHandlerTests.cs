using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for the <see cref="GetSaleHandler"/> class, validating sale retrieval scenarios.
/// </summary>
public class GetSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly GetSaleHandler _handler;

    /// <summary>
    /// Initializes the handler and its dependencies.
    /// </summary>
    public GetSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetSaleHandler(_saleRepository, _mapper);
    }

    /// <summary>
    /// Ensures that a valid ID returns the corresponding sale details.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When retrieving sale Then returns sale details")]
    public async Task Handle_ValidId_ReturnsSaleDetails()
    {
        // Arrange
        var saleId = Guid.NewGuid();
        var command = new GetSaleCommand { Id = saleId };

        var sale = new Sale
        {
            Id = saleId,
            SaleNumber = "SALE-123",
            Customer = "João da Silva",
            Branch = "Filial São Paulo",
            SaleDate = DateTime.UtcNow,
            IsCancelled = false
        };

        var result = new GetSaleResult
        {
            Id = sale.Id,
            SaleNumber = sale.SaleNumber,
            Customer = sale.Customer,
            Branch = sale.Branch,
            SaleDate = sale.SaleDate,
            IsCancelled = sale.IsCancelled
        };

        _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
        _mapper.Map<GetSaleResult>(sale).Returns(result);

        // Act
        var response = await _handler.Handle(command, CancellationToken.None);

        // Assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sale.Id);
        response.SaleNumber.Should().Be(sale.SaleNumber);
        response.Customer.Should().Be(sale.Customer);
        response.Branch.Should().Be(sale.Branch);
        response.SaleDate.Should().Be(sale.SaleDate);
        response.IsCancelled.Should().BeFalse();
    }

    /// <summary>
    /// Ensures that a request with an invalid ID throws a <see cref="KeyNotFoundException"/>.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When retrieving sale Then throws KeyNotFoundException")]
    public async Task Handle_InvalidId_ThrowsKeyNotFoundException()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        var command = new GetSaleCommand { Id = invalidId };

        _saleRepository.GetByIdAsync(invalidId, Arg.Any<CancellationToken>())
            .Returns((Sale?)null);

        // Act
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {invalidId} not found");
    }
}
