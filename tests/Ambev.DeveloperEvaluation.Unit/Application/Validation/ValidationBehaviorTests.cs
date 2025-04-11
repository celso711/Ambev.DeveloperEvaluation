using Xunit;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Common.Validation;
using NSubstitute;

public class FakeCommand : IRequest<string> { public string Name { get; set; } }

public class FakeCommandValidator : AbstractValidator<FakeCommand>
{
    public FakeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}

public class ValidationBehaviorTests
{
    [Fact]
    public async Task Should_Throw_When_Validation_Fails()
    {
        // Arrange
        var validators = new List<IValidator<FakeCommand>> { new FakeCommandValidator() };
        var behavior = new ValidationBehavior<FakeCommand, string>(validators);

        var request = new FakeCommand { Name = "" };
        var next = Substitute.For<RequestHandlerDelegate<string>>();

        // Act & Assert
        await Assert.ThrowsAsync<ValidationException>(() =>
            behavior.Handle(request, next, CancellationToken.None));
    }

    [Fact]
    public async Task Should_Continue_When_Validation_Succeeds()
    {
        // Arrange
        var validators = new List<IValidator<FakeCommand>> { new FakeCommandValidator() };
        var behavior = new ValidationBehavior<FakeCommand, string>(validators);

        var request = new FakeCommand { Name = "Valid" };
        var next = Substitute.For<RequestHandlerDelegate<string>>();
        next().Returns("Success");

        // Act
        var result = await behavior.Handle(request, next, CancellationToken.None);

        // Assert
        Assert.Equal("Success", result);
    }
}
