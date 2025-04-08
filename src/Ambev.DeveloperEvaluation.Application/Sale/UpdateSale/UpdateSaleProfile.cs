using AutoMapper;

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
            CreateMap<UpdateSaleCommand, Ambev.DeveloperEvaluation.Domain.Entities.Sale>();
            CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.Sale, UpdateSaleResult>();
        }
    }
}
