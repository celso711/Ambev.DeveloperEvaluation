using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Security;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.WebApi.Extensions
{
    public class AuthenticationExtensionTests
    {
        [Fact]
        public void Should_Add_Jwt_Authentication()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:SecretKey", "ThisIsASecretKeyThatIsLongEnoughToBeValid"}
        };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddJwtAuthentication(configuration);

            var provider = services.BuildServiceProvider();
            Assert.NotNull(provider.GetService<IJwtTokenGenerator>());
        }
    }
}
