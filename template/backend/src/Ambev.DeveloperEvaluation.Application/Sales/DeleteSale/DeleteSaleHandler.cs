using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.DeleteSale
{
    public class DeleteSaleHandler : IRequestHandler<DeleteSaleCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;

        public DeleteSaleHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

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
