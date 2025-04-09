using Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.AddSaleItem
{
    /// <summary>
    /// Profile for mapping between Application and API AddSaleItem responses
    /// </summary>
    public class AddSaleItemProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for AddSaleItem feature
        /// </summary>
        public AddSaleItemProfile()
        {
            CreateMap<AddSaleItemRequest, AddSaleItemCommand>();
            CreateMap<AddSaleItemCommand, AddSaleItemResponse>();

            CreateMap<AddSaleItemCommand, SaleItem>()
                .ForMember(dest => dest.Discount, opt => opt.Ignore())
                .ForMember(dest => dest.Total, opt => opt.Ignore());

            CreateMap<SaleItem, AddSaleItemResponse>();
        }
    }
}
