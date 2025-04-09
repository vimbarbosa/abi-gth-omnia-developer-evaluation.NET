using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Defines AutoMapper mappings for updating a sale.
    /// Maps between <see cref="UpdateSaleCommand"/>, <see cref="Sale"/>, and <see cref="UpdateSaleResult"/>.
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateSaleProfile"/> class.
        /// </summary>
        public UpdateSaleProfile()
        {
            // Maps properties from UpdateSaleCommand to Sale entity.
            CreateMap<UpdateSaleCommand, Sale>();

            // Maps properties from Sale entity to UpdateSaleResult.
            CreateMap<Sale, UpdateSaleResult>();
        }
    }
}
