using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and GetSaleResponse
    /// </summary>
    public sealed class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<SaleItem, GetSaleItemResult>();

            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));
        }
    }
}
