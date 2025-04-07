using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale
{
    /// <summary>
    /// Profile for mapping between Sale entity and GetSaleResponse
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetSale operation
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Ambev.DeveloperEvaluation.Domain.Entities.Sale, GetSaleResult>();
        }
    }
}
