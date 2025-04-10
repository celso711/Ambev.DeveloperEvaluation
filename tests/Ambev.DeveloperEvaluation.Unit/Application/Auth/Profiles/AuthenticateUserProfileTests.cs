using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using AutoMapper;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth.Profiles
{
    /// <summary>
    /// Tests AutoMapper configuration for AuthenticateUserProfile.
    /// </summary>
    public class AuthenticateUserProfileTests
    {
        [Fact(DisplayName = "AuthenticateUserProfile mapping configuration should be valid")]
        public void AutoMapperConfiguration_ShouldBeValid()
        {
            // Arrange
            var config = new MapperConfiguration(cfg => cfg.AddProfile<AuthenticateUserProfile>());

            // Assert
            config.AssertConfigurationIsValid();
        }
    }
}
