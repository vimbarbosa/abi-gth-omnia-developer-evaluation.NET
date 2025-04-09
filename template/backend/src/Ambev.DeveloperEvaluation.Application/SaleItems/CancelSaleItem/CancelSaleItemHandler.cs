using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.CancelSaleItem
{
    public class CancelSaleItemHandler : IRequestHandler<CancelSaleItemCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        public CancelSaleItemHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        public async Task<Unit> Handle(CancelSaleItemCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleItemValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found.");

            var item = sale.Items.FirstOrDefault(i => i.Id == command.ItemId);
            if (item == null)
                throw new KeyNotFoundException($"Item with ID {command.ItemId} not found in sale.");

            item.IsCancelled = true;

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish Event
            await _eventPublisher.PublishAsync(new CancelSaleItemEvent { Payload = item }, "sales.events", cancellationToken);

            return Unit.Value;
        }
    }
}
