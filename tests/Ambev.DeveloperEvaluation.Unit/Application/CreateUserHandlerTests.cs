using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Domain;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application;

/// <summary>
/// Contains unit tests for the <see cref="CreateUserHandler"/> class.
/// </summary>
public class CreateUserHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly CreateUserHandler _handler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CreateUserHandlerTests"/> class.
    /// Sets up the test dependencies and creates fake data generators.
    /// </summary>
    public CreateUserHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _mapper = Substitute.For<IMapper>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _handler = new CreateUserHandler(_userRepository, _mapper, _passwordHasher);
    }

    [Fact(DisplayName = "Should throw ValidationException when command is invalid")]
    public async Task Handle_WithInvalidCommand_ShouldThrowValidationException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "", // inválido
            Email = "invalid-email",
            Password = "123", // inválido
            Phone = "123",
            Role = UserRole.None,
            Status = UserStatus.Unknown
        };

        var userRepository = Substitute.For<IUserRepository>();
        var passwordHasher = Substitute.For<IPasswordHasher>();
        var mapper = Substitute.For<IMapper>();

        var handler = new CreateUserHandler(userRepository, mapper, passwordHasher);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    [Fact(DisplayName = "Should throw when email is already registered")]
    public async Task Handle_WithExistingEmail_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "jane",
            Email = "jane@example.com",
            Password = "Test@123",
            Phone = "+5511999999999",
            Role = UserRole.Admin,
            Status = UserStatus.Active
        };

        var existingUser = new User { Email = command.Email };

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                      .Returns(existingUser);

        var passwordHasher = Substitute.For<IPasswordHasher>();
        var mapper = Substitute.For<IMapper>();

        var handler = new CreateUserHandler(userRepository, mapper, passwordHasher);

        // Act
        var act = async () => await handler.Handle(command, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage($"User with email {command.Email} already exists");
    }

    [Fact(DisplayName = "Should create a new user when data is valid and email not used")]
    public async Task Handle_WithValidCommand_ShouldCreateUser()
    {
        // Arrange
        var command = new CreateUserCommand
        {
            Username = "john_doe",
            Email = "john@example.com",
            Password = "Test@123",
            Phone = "+5511999999999",
            Role = UserRole.Customer,
            Status = UserStatus.Active
        };

        var userRepository = Substitute.For<IUserRepository>();
        userRepository.GetByEmailAsync(command.Email, Arg.Any<CancellationToken>())
                      .Returns((User)null);

        var passwordHasher = Substitute.For<IPasswordHasher>();
        passwordHasher.HashPassword(command.Password).Returns("hashed-password");

        var createdUser = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Email = command.Email,
            Password = "hashed-password",
            Role = command.Role,
            Status = command.Status
        };

        userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
                      .Returns(createdUser);

        var mapper = Substitute.For<IMapper>();
        mapper.Map<User>(command).Returns(createdUser);
        mapper.Map<CreateUserResult>(createdUser).Returns(new CreateUserResult { Id = createdUser.Id });

        var handler = new CreateUserHandler(userRepository, mapper, passwordHasher);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(createdUser.Id);
    }
    /// <summary>
    /// Tests that a valid user creation request is handled successfully.
    /// </summary>
    [Fact(DisplayName = "Given valid user data When creating user Then returns success response")]
    public async Task Handle_ValidRequest_ReturnsSuccessResponse()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role
        };

        var result = new CreateUserResult
        {
            Id = user.Id,
        };


        _mapper.Map<User>(command).Returns(user);
        _mapper.Map<CreateUserResult>(user).Returns(result);

        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        var createUserResult = await _handler.Handle(command, CancellationToken.None);

        // Then
        createUserResult.Should().NotBeNull();
        createUserResult.Id.Should().Be(user.Id);
        await _userRepository.Received(1).CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that an invalid user creation request throws a validation exception.
    /// </summary>
    [Fact(DisplayName = "Given invalid user data When creating user Then throws validation exception")]
    public async Task Handle_InvalidRequest_ThrowsValidationException()
    {
        // Given
        var command = new CreateUserCommand(); // Empty command will fail validation

        // When
        var act = () => _handler.Handle(command, CancellationToken.None);

        // Then
        await act.Should().ThrowAsync<FluentValidation.ValidationException>();
    }

    /// <summary>
    /// Tests that the password is hashed before saving the user.
    /// </summary>
    [Fact(DisplayName = "Given user creation request When handling Then password is hashed")]
    public async Task Handle_ValidRequest_HashesPassword()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var originalPassword = command.Password;
        const string hashedPassword = "h@shedPassw0rd";
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role
        };

        _mapper.Map<User>(command).Returns(user);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(originalPassword).Returns(hashedPassword);

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _passwordHasher.Received(1).HashPassword(originalPassword);
        await _userRepository.Received(1).CreateAsync(
            Arg.Is<User>(u => u.Password == hashedPassword),
            Arg.Any<CancellationToken>());
    }

    /// <summary>
    /// Tests that the mapper is called with the correct command.
    /// </summary>
    [Fact(DisplayName = "Given valid command When handling Then maps command to user entity")]
    public async Task Handle_ValidRequest_MapsCommandToUser()
    {
        // Given
        var command = CreateUserHandlerTestData.GenerateValidCommand();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Username = command.Username,
            Password = command.Password,
            Email = command.Email,
            Phone = command.Phone,
            Status = command.Status,
            Role = command.Role
        };

        _mapper.Map<User>(command).Returns(user);
        _userRepository.CreateAsync(Arg.Any<User>(), Arg.Any<CancellationToken>())
            .Returns(user);
        _passwordHasher.HashPassword(Arg.Any<string>()).Returns("hashedPassword");

        // When
        await _handler.Handle(command, CancellationToken.None);

        // Then
        _mapper.Received(1).Map<User>(Arg.Is<CreateUserCommand>(c =>
            c.Username == command.Username &&
            c.Email == command.Email &&
            c.Phone == command.Phone &&
            c.Status == command.Status &&
            c.Role == command.Role));
    }
}
