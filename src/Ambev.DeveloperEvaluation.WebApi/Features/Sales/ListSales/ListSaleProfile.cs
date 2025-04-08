using Ambev.DeveloperEvaluation.Application.Sales.ListSale;
using Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateUser feature
        /// </summary>
        public ListSaleProfile()
        {
            CreateMap<ListSaleRequest, ListSaleCommand>();

            CreateMap<Application.Sales.GetSale.GetSaleResult, GetSaleResponse>();
        }
    }
}
