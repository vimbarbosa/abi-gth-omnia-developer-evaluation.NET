using Ambev.DeveloperEvaluation.Application.SaleItems.Dtos;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Defines AutoMapper profile for mapping domain entities to the result DTO used in the GetSale query.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GetSaleProfile"/> class.
        /// Configures mappings from <see cref="Sale"/> to <see cref="GetSaleResult"/>,
        /// and from <see cref="SaleItem"/> to <see cref="SaleItemDto"/>.
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Sale, GetSaleResult>();
            CreateMap<SaleItem, SaleItemDto>();
        }
    }
}
