using AutoMapper;
using MediatR;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Handles the retrieval of a sale by its ID.
    /// </summary>
    public class GetSaleHandler : IRequestHandler<GetSaleCommand, GetSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The repository used to retrieve the sale.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities to results.</param>
        public GetSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the query to get a sale by ID.
        /// </summary>
        /// <param name="command">The command containing the sale ID.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The result containing sale details.</returns>
        /// <exception cref="ValidationException">Thrown if the provided ID is invalid.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the sale is not found.</exception>
        public async Task<GetSaleResult> Handle(GetSaleCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == Guid.Empty)
                throw new ValidationException("Invalid sale ID");

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (sale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found");

            return _mapper.Map<GetSaleResult>(sale);
        }
    }
}
