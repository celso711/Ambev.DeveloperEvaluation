using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Auth
{
    public class GetUserHandlerTests
    {
        [Fact(DisplayName = "Should throw ValidationException when ID is empty")]
        public async Task Handle_WithInvalidCommand_ThrowsValidationException()
        {
            // Arrange
            var command = new GetUserCommand(Guid.Empty);

            var repo = Substitute.For<IUserRepository>();
            var mapper = Substitute.For<IMapper>();

            var handler = new GetUserHandler(repo, mapper);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<FluentValidation.ValidationException>();
        }

        [Fact(DisplayName = "Should throw KeyNotFoundException when user is not found")]
        public async Task Handle_WithNonexistentUser_ThrowsKeyNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new GetUserCommand(userId);

            var repo = Substitute.For<IUserRepository>();
            repo.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns((User)null);

            var mapper = Substitute.For<IMapper>();
            var handler = new GetUserHandler(repo, mapper);

            // Act
            var act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<KeyNotFoundException>()
                .WithMessage($"User with ID {userId} not found");
        }


        [Fact(DisplayName = "Should return user when found by ID")]
        public async Task Handle_WithValidId_ReturnsMappedUserResult()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var command = new GetUserCommand(userId);

            var user = new User
            {
                Id = userId,
                Username = "john",
                Email = "john@example.com",
                Phone = "+5511999999999",
                Role = UserRole.Admin,
                Status = UserStatus.Active
            };

            var expectedResult = new GetUserResult
            {
                Id = userId,
                Name = user.Username,
                Email = user.Email,
                Phone = user.Phone,
                Role = user.Role,
                Status = user.Status
            };

            var repo = Substitute.For<IUserRepository>();
            repo.GetByIdAsync(userId, Arg.Any<CancellationToken>()).Returns(user);

            var mapper = Substitute.For<IMapper>();
            mapper.Map<GetUserResult>(user).Returns(expectedResult);

            var handler = new GetUserHandler(repo, mapper);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
    
