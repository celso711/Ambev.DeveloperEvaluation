using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using FluentAssertions;
using NSubstitute;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class AuthenticateUserHandlerTests
    {
        [Fact(DisplayName = "Given inactive user, should throw UnauthorizedAccessException")]
        public async Task Handle_WithInactiveUser_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            user.Status = UserStatus.Suspended;

            var command = new AuthenticateUserCommand
            {
                Email = user.Email,
                Password = user.Password
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                          .Returns(user);

            var passwordHasher = Substitute.For<IPasswordHasher>();
            passwordHasher.VerifyPassword(command.Password, user.Password).Returns(true);

            var jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

            var handler = new AuthenticateUserHandler(userRepository, passwordHasher, jwtTokenGenerator);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("User is not active");
        }

        [Fact(DisplayName = "Given invalid password, should throw UnauthorizedAccessException")]
        public async Task Handle_WithInvalidPassword_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var user = UserTestData.GenerateValidUser();
            user.Status = UserStatus.Active;

            var command = new AuthenticateUserCommand
            {
                Email = user.Email,
                Password = "wrongpassword"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                          .Returns(user);

            var passwordHasher = Substitute.For<IPasswordHasher>();
            passwordHasher.VerifyPassword(command.Password, user.Password).Returns(false);

            var jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

            var handler = new AuthenticateUserHandler(userRepository, passwordHasher, jwtTokenGenerator);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials");
        }

        [Fact(DisplayName = "Given nonexistent user, should throw UnauthorizedAccessException")]
        public async Task Handle_WithNonexistentUser_ShouldThrowUnauthorizedAccessException()
        {
            // Arrange
            var command = new AuthenticateUserCommand
            {
                Email = "nonexistent@example.com",
                Password = "irrelevant"
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                          .Returns((User)null);

            var passwordHasher = Substitute.For<IPasswordHasher>();
            var jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();

            var handler = new AuthenticateUserHandler(userRepository, passwordHasher, jwtTokenGenerator);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<UnauthorizedAccessException>()
                .WithMessage("Invalid credentials");
        }
        [Fact(DisplayName = "Given valid credentials, handler should return token")]
        public async Task Handle_WithValidCredentials_ReturnsAuthenticateUserResult()
        {
            // Arrange
            var validUser = UserTestData.GenerateValidUser();
            validUser.Status = UserStatus.Active;

            var command = new AuthenticateUserCommand
            {
                Email = validUser.Email,
                Password = validUser.Password
            };

            var userRepository = Substitute.For<IUserRepository>();
            userRepository
                .GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                .Returns(validUser);

            var passwordHasher = Substitute.For<IPasswordHasher>();
            passwordHasher
                .VerifyPassword(command.Password, validUser.Password)
                .Returns(true);

            var jwtTokenGenerator = Substitute.For<IJwtTokenGenerator>();
            jwtTokenGenerator
                .GenerateToken(validUser)
                .Returns("test-jwt-token");

            var handler = new AuthenticateUserHandler(userRepository, passwordHasher, jwtTokenGenerator);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Token.Should().Be("test-jwt-token");
            result.Email.Should().Be(validUser.Email);
            result.Name.Should().Be(validUser.Username);
            result.Role.Should().Be(validUser.Role.ToString());
        }
    }
}
