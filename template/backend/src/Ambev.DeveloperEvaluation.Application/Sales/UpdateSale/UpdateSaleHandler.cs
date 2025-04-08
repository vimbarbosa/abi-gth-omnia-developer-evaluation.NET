using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Events;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using OneOf.Types;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly IEventPublisher _eventPublisher;

        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _eventPublisher = eventPublisher;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            command.Discount = Sale.CalculateDiscount(command.Quantity, command.UnitPrice);
            command.Total = (command.UnitPrice * command.Quantity) - command.Discount;

            _mapper.Map(command, sale);

            var updated = await _saleRepository.UpdateAsync(sale, cancellationToken);

            // Publish Event
            await _eventPublisher.PublishAsync(new { Event = "SaleModified", Payload = updated }, "sales.events", cancellationToken);

            return _mapper.Map<UpdateSaleResult>(updated);
        }
    }

}
