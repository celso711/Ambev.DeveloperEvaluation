using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.Profiles
{
    /// <summary>
    /// Tests AutoMapper configuration for GetSaleProfile.
    /// </summary>
    public class GetSaleProfileTests
    {
        [Fact(DisplayName = "GetSaleProfile mapping configuration should be valid")]
        public void AutoMapperConfiguration_ShouldBeValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<GetSaleProfile>());

            // Assert
            config.AssertConfigurationIsValid();
        }
    }
}
