using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.Profiles
{
    public class UpdateSaleProfileTests
    {
        [Fact]
        public void Should_Have_Valid_Configuration()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<UpdateSaleProfile>());
            config.AssertConfigurationIsValid();
        }

    }
}
