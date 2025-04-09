using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handles the logic for updating an existing sale and publishing a domain event.
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">Repository used for accessing sale data.</param>
        /// <param name="mapper">AutoMapper instance for mapping between models.</param>
        /// <param name="eventPublisher">Publisher used to emit domain events.</param>
        public UpdateSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the update sale command by validating the input, updating the entity,
        /// saving changes to the database, and publishing a domain event.
        /// </summary>
        /// <param name="command">The command containing the updated sale data.</param>
        /// <param name="cancellationToken">A token for cancelling the operation.</param>
        /// <returns>An updated sale result.</returns>
        /// <exception cref="ValidationException">Thrown when validation fails.</exception>
        /// <exception cref="KeyNotFoundException">Thrown when the sale does not exist.</exception>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            _mapper.Map(command, sale);

            var updated = await _saleRepository.UpdateAsync(sale, cancellationToken);

            await _eventPublisher.PublishAsync(
                new SaleModifiedEvent { Payload = updated },
                "sales.events",
                cancellationToken);

            return _mapper.Map<UpdateSaleResult>(updated);
        }
    }
}
