using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    /// <summary>
    /// Contains unit tests for the <see cref="DeleteSaleHandler"/> class.
    /// </summary>
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly DeleteSaleHandler _handler;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSaleHandlerTests"/> class.
        /// Sets up the mocked dependencies.
        /// </summary>
        public DeleteSaleHandlerTests()
        {
            _saleRepository = Substitute.For<ISaleRepository>();
            _mapper = Substitute.For<IMapper>();
            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact(DisplayName = "Given valid sale ID When deleting sale Then returns success")]
        public async Task Handle_ValidId_DeletesSuccessfully()
        {
            // Given
            var id = Guid.NewGuid();
            var command = new DeleteSaleCommand { Id = id };

            _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(true);
            _saleRepository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

            // When
            var result = await _handler.Handle(command, CancellationToken.None);

            // Then
            result.Should().Be(MediatR.Unit.Value);
            await _saleRepository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
        }

        [Fact(DisplayName = "Given non-existent sale ID When deleting sale Then throws KeyNotFoundException")]
        public async Task Handle_NonExistentId_ThrowsException()
        {
            // Given
            var id = Guid.NewGuid();
            var command = new DeleteSaleCommand { Id = id };

            _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(false);

            // When
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"Sale with ID {id} not found");
        }

        [Fact(DisplayName = "Given repository throws exception When deleting sale Then propagates exception")]
        public async Task Handle_RepositoryThrows_ThrowsException()
        {
            // Given
            var id = Guid.NewGuid();
            var command = new DeleteSaleCommand { Id = id };

            _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(true);
            _saleRepository.DeleteAsync(id, Arg.Any<CancellationToken>())
                .Throws(new Exception("Unexpected failure"));

            // When
            Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

            // Then
            await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected failure");
        }
    }
}
