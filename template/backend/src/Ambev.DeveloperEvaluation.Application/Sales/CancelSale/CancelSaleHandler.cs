using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    /// <summary>
    /// Handles the cancellation of a sale.
    /// Validates the command, updates the sale's cancellation status, and publishes an event.
    /// </summary>
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, Unit>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The sale repository to access sales data.</param>
        /// <param name="eventPublisher">The event publisher to notify other systems of the cancellation.</param>
        public CancelSaleHandler(ISaleRepository saleRepository, IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the sale cancellation request.
        /// </summary>
        /// <param name="command">The cancellation command containing the sale ID.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A <see cref="Unit"/> value upon successful cancellation.</returns>
        /// <exception cref="ValidationException">Thrown if the command validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the sale with the specified ID does not exist.</exception>
        public async Task<Unit> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found.");

            sale.IsCancelled = true;

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish cancellation event
            await _eventPublisher.PublishAsync(
                new CancelSaleEvent { Payload = sale },
                "sales.events",
                cancellationToken
            );

            return Unit.Value;
        }
    }
}
