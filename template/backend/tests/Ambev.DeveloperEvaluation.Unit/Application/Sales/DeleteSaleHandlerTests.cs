using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales;

/// <summary>
/// Unit tests for the <see cref="DeleteSaleHandler"/> responsible for deleting sales.
/// </summary>
public class DeleteSaleHandlerTests
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly DeleteSaleHandler _handler;

    /// <summary>
    /// Initializes the test setup with mocked dependencies.
    /// </summary>
    public DeleteSaleHandlerTests()
    {
        _saleRepository = Substitute.For<ISaleRepository>();
        _mapper = Substitute.For<IMapper>();
        _handler = new DeleteSaleHandler(_saleRepository);
    }

    /// <summary>
    /// Verifies that a valid sale ID deletes the sale successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid sale ID When deleting sale Then returns success")]
    public async Task Handle_ValidId_DeletesSuccessfully()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteSaleCommand { Id = id };

        _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(true);
        _saleRepository.DeleteAsync(id, Arg.Any<CancellationToken>()).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(MediatR.Unit.Value);
        await _saleRepository.Received(1).DeleteAsync(id, Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Verifies that an exception is thrown when the sale ID does not exist.
    /// </summary>
    [Fact(DisplayName = "Given non-existent sale ID When deleting sale Then throws KeyNotFoundException")]
    public async Task Handle_NonExistentId_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteSaleCommand { Id = id };

        _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(false);

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<KeyNotFoundException>()
            .WithMessage($"Sale with ID {id} not found");
    }

    /// <summary>
    /// Verifies that exceptions thrown by the repository are propagated.
    /// </summary>
    [Fact(DisplayName = "Given repository throws exception When deleting sale Then propagates exception")]
    public async Task Handle_RepositoryThrows_ThrowsException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new DeleteSaleCommand { Id = id };

        _saleRepository.ExistsAsync(id, Arg.Any<CancellationToken>()).Returns(true);
        _saleRepository.DeleteAsync(id, Arg.Any<CancellationToken>())
            .Throws(new Exception("Unexpected failure"));

        // Act
        Func<Task> act = () => _handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<Exception>().WithMessage("Unexpected failure");
    }
}
