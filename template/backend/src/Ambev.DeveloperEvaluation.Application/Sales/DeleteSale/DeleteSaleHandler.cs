using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    /// <summary>
    /// Handles the deletion of a sale based on the provided command.
    /// </summary>
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The repository responsible for managing sales.</param>
        public DeleteSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        /// <summary>
        /// Handles the logic for deleting a sale.
        /// </summary>
        /// <param name="command">The command containing the sale ID to delete.</param>
        /// <param name="cancellationToken">Token to cancel the operation.</param>
        /// <returns>A <see cref="Unit"/> value representing the completion of the operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the sale does not exist.</exception>
        public async Task<Unit> Handle(DeleteSaleCommand command, CancellationToken cancellationToken)
        {
            var exists = await _saleRepository.ExistsAsync(command.Id, cancellationToken);
            if (!exists)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            await _saleRepository.DeleteAsync(command.Id, cancellationToken);
            return Unit.Value;
        }
    }
}
