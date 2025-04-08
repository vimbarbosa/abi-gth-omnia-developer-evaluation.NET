using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    /// <summary>
    /// Contains unit tests for the <see cref="GetSaleHandler"/> class.
    /// </summary>
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly GetSaleHandler _handler;

        public GetSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new GetSaleHandler(_saleRepository, _mapper);
        }

        [Fact(DisplayName = "Given valid sale ID When retrieving sale Then returns sale details")]
        public async Task Handle_ValidId_ReturnsSaleDetails()
        {
            // Given
            var saleId = Guid.NewGuid();
            var command = new GetSaleCommand { Id = saleId };

            var sale = new Sale
            {
                Id = saleId,
                SaleNumber = "SALE-123",
                Customer = "João da Silva",
                Branch = "Filial São Paulo",
                SaleDate = DateTime.UtcNow,
                Product = "Produto Teste",
                Quantity = 2,
                UnitPrice = 150,
                Discount = 10,
                Total = 290,
                IsCancelled = false
            };

            var result = new GetSaleResult
            {
                Id = sale.Id,
                SaleNumber = sale.SaleNumber,
                Customer = sale.Customer,
                Branch = sale.Branch,
                SaleDate = sale.SaleDate,
                Product = sale.Product,
                Quantity = sale.Quantity,
                UnitPrice = sale.UnitPrice,
                Discount = sale.Discount,
                Total = sale.Total,
                IsCancelled = sale.IsCancelled
            };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
            _mapper.Map<GetSaleResult>(sale).Returns(result);

            // When
            var response = await _handler.Handle(command, CancellationToken.None);

            // Then
            response.Should().NotBeNull();
            response.Id.Should().Be(sale.Id);
            response.SaleNumber.Should().Be(sale.SaleNumber);
            response.Customer.Should().Be(sale.Customer);
            response.Branch.Should().Be(sale.Branch);
            response.SaleDate.Should().Be(sale.SaleDate);
            response.Product.Should().Be(sale.Product);
            response.Quantity.Should().Be(sale.Quantity);
            response.UnitPrice.Should().Be(sale.UnitPrice);
            response.Discount.Should().Be(sale.Discount);
            response.Total.Should().Be(sale.Total);
            response.IsCancelled.Should().BeFalse();
        }

        [Fact(DisplayName = "Given non-existent sale ID When retrieving sale Then throws KeyNotFoundException")]
        public async Task Handle_InvalidId_ThrowsKeyNotFoundException()
        {
            // Given
            var invalidId = Guid.NewGuid();
            var command = new GetSaleCommand { Id = invalidId };

            _saleRepository.GetByIdAsync(invalidId, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // When
            var act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {invalidId} not found");
        }
    }
}
