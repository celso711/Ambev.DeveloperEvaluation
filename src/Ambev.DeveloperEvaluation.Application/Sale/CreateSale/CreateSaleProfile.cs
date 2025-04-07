using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale
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
            CreateMap<CreateSaleCommand, Ambev.DeveloperEvaluation.Domain.Entities.Sale >();
            CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.Sale, CreateSaleResult>();

            CreateMap<SaleItemCommand, SaleItem>();
        }
    }
}
