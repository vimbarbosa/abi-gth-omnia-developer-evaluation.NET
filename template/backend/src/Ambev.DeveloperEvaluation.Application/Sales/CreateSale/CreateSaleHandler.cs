using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Domain.Events;

public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
{
    private readonly ISaleRepository _saleRepository;
    private readonly IMapper _mapper;
    private readonly IEventPublisher _eventPublisher;

    public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper, IEventPublisher eventPublisher)
    {
        _saleRepository = saleRepository;
        _mapper = mapper;
        _eventPublisher = eventPublisher;
    }

    public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateSaleValidator();
        var validationResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        command.Discount = Sale.CalculateDiscount(command.Quantity, command.UnitPrice);
        command.Total = (command.UnitPrice * command.Quantity) - command.Discount;

        var entity = _mapper.Map<Sale>(command);
        var createdSale = await _saleRepository.CreateAsync(entity, cancellationToken);
        var result = _mapper.Map<CreateSaleResult>(createdSale);

        // Publish Event
        await _eventPublisher.PublishAsync(new SaleCreatedEvent { Payload = result }, "sales.events", cancellationToken);

        return result;
    }
}