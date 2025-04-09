using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.SaleItems.AddSaleItem
{
    /// <summary>
    /// AutoMapper profile for mapping between AddSaleItemCommand and SaleItem.
    /// </summary>
    public class AddSaleItemProfile : Profile
    {
        public AddSaleItemProfile()
        {
            CreateMap<AddSaleItemCommand, SaleItem>()
                .ForMember(dest => dest.Total,
                           opt => opt.MapFrom(src => (src.UnitPrice * src.Quantity) - SaleItem.CalculateDiscount(src.Quantity, src.UnitPrice)))
                .ForMember(dest => dest.Discount,
                           opt => opt.MapFrom(src => SaleItem.CalculateDiscount(src.Quantity, src.UnitPrice)));

            CreateMap<SaleItem, AddSaleItemResult>();
        }
    }
}
