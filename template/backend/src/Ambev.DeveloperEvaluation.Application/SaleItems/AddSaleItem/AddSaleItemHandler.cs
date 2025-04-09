using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem
{
    public class AddSaleItemHandler : IRequestHandler<AddSaleItemCommand, AddSaleItemResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        public AddSaleItemHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        public async Task<AddSaleItemResult> Handle(AddSaleItemCommand command, CancellationToken cancellationToken)
        {
            // Busca a venda existente
            var sale = await _saleRepository.GetByIdAsync(command.SaleId, cancellationToken);
            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.SaleId} not found");

            if (command.Quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 identical items.");

            var item = new SaleItem
            {
                SaleId = command.SaleId,
                Product = command.Product,
                Quantity = command.Quantity,
                UnitPrice = command.UnitPrice,
                IsCancelled = false
            };

            sale.Items.Add(item);

            await _saleRepository.UpdateAsync(sale, cancellationToken);

            return _mapper.Map<AddSaleItemResult>(item);
        }
    }
}
