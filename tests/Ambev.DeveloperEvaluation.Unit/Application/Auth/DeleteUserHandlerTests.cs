using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class DeleteUserHandlerTests
    {
        [Fact(DisplayName = "Should throw ValidationException when ID is empty")]
        public async Task Handle_WithEmptyGuid_ShouldThrowValidationException()
        {
            // Arrange
            var command = new DeleteUserCommand(Guid.Empty);

            var repo = Substitute.For<IUserRepository>();
            var handler = new DeleteUserHandler(repo);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }


        [Fact(DisplayName = "Should throw KeyNotFoundException when user is not found")]
        public async Task Handle_WithNonExistentUser_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand(userId);

            var repo = Substitute.For<IUserRepository>();
            repo.DeleteAsync(userId, Arg.Any<CancellationToken>()).Returns(false);

            var handler = new DeleteUserHandler(repo);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID {userId} not found");
        }

        [Fact(DisplayName = "Should return success when user is deleted")]
        public async Task Handle_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new DeleteUserCommand(userId);

            var repo = Substitute.For<IUserRepository>();
            repo.DeleteAsync(userId, Arg.Any<CancellationToken>()).Returns(true);

            var handler = new DeleteUserHandler(repo);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
        }
    }
}
