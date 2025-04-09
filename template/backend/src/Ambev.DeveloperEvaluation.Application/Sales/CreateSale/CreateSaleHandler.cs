using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handles the creation of a new sale, including its items and event publishing.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">Repository for accessing and persisting sales.</param>
        /// <param name="mapper">Object mapper for converting between command and entity/result.</param>
        /// <param name="eventPublisher">Publisher for emitting domain events.</param>
        public CreateSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        /// <summary>
        /// Handles the CreateSaleCommand by validating, creating the sale, and publishing an event.
        /// </summary>
        /// <param name="command">The command containing sale data.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>A result containing the created sale details.</returns>
        /// <exception cref="ValidationException">Thrown when the command is invalid.</exception>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var entity = _mapper.Map<Sale>(command);
            var createdSale = await _saleRepository.CreateAsync(entity, cancellationToken);
            var result = _mapper.Map<CreateSaleResult>(createdSale);

            // Publish Event
            await _eventPublisher.PublishAsync(new SaleCreatedEvent
            {
                Payload = createdSale
            }, "sales.events", cancellationToken);

            return result;
        }
    }
}
