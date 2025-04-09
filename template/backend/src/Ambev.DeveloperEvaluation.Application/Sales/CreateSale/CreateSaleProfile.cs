using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Defines mapping configuration for creating a sale.
    /// Maps between CreateSaleCommand and Sale entity, and Sale to CreateSaleResult.
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class.
        /// Sets up AutoMapper configurations.
        /// </summary>
        public CreateSaleProfile()
        {
            // Maps command to entity
            CreateMap<CreateSaleCommand, Sale>();

            // Maps entity to result DTO
            CreateMap<Sale, CreateSaleResult>();
        }
    }
}
