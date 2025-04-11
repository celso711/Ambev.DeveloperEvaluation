using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CreateSaleResponse
    /// </summary>
    public class UpdateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for UpdateSale operation
        /// </summary>
        public UpdateSaleProfile()
        {

            CreateMap<UpdateSaleCommand, Sale>()
            .ForMember(dest => dest.CustomerId, opt => opt.MapFrom(src => Guid.Parse(src.CustomerId)))
            .ForMember(dest => dest.CustomerName, opt => opt.MapFrom(src => src.CustomerName))
            .ForMember(dest => dest.BranchId, opt => opt.MapFrom(src => Guid.Parse(src.BranchId)))
            .ForMember(dest => dest.BranchName, opt => opt.MapFrom(src => src.BranchName))
            .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
            .ForMember(dest => dest.SaleNumber, opt => opt.Ignore())
            .ForMember(dest => dest.SaleDate, opt => opt.Ignore())
            .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
            .ForMember(dest => dest.IsCancelled, opt => opt.Ignore());

            CreateMap<UpdateSaleItemCommand, SaleItem>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
                .ForMember(dest => dest.Discount, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.SaleId, opt => opt.Ignore());

        }
    }
}
