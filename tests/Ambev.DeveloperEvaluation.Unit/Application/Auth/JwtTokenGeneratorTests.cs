using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class JwtTokenGeneratorTests
    {
        [Fact(DisplayName = "Should generate JWT token with expected claims")]
        public void GenerateToken_WithValidUser_ReturnsValidJwt()
        {
            // Arrange
            var secretKey = "super_secret_key_which_is_long_enough";
            var configuration = Substitute.For<IConfiguration>();
            configuration["Jwt:SecretKey"].Returns(secretKey);

            var tokenGenerator = new JwtTokenGenerator(configuration);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "john_doe",
                Role = UserRole.Admin
            };

            // Act
            var token = tokenGenerator.GenerateToken(user);

            // Assert
            token.Should().NotBeNullOrWhiteSpace();

            // Desativa o mapeamento automático que converte o tipo dos claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);
            foreach (var claim in jwt.Claims)
            {
                Console.WriteLine($"Claim Type: {claim.Type}, Value: {claim.Value}");
            }
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == user.Id.ToString());
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == user.Username);
            jwt.Claims.Should().Contain(c => c.Type == ClaimTypes.Role && c.Value == user.Role.ToString());

            jwt.ValidTo.Should().BeAfter(DateTime.UtcNow);
            jwt.ValidTo.Should().BeBefore(DateTime.UtcNow.AddHours(9));
        }
    }
}
