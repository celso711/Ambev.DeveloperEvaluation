using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sale.Profiles
{
    /// <summary>
    /// Tests AutoMapper configuration for CreateSaleProfile.
    /// </summary>
    public class CreateSaleProfileTests
    {
        [Fact(DisplayName = "CreateSaleProfile mapping configuration should be valid")]
        public void AutoMapperConfiguration_ShouldBeValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CreateSaleProfile>());

            // Assert
            config.AssertConfigurationIsValid();
        }
    }
}
