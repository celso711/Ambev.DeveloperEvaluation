using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and CreateSaleResponse
    /// </summary>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for CreateSale operation
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Ambev.DeveloperEvaluation.Domain.Entities.Sale>();
            CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.Sale, CreateSaleResult>();

            CreateMap<SaleItemCommand, SaleItem>();
        }
    }
}
