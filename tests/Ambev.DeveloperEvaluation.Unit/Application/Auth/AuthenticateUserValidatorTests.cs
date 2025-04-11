using Ambev.DeveloperEvaluation.Application.Auth.AuthenticateUser;
using FluentValidation.TestHelper;
using Xunit;

public class AuthenticateUserValidatorTests
{
    private readonly AuthenticateUserValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var result = _validator.TestValidate(new AuthenticateUserCommand { Email = "", Password = "123" });
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid()
    {
        var result = _validator.TestValidate(new AuthenticateUserCommand { Email = "test@email.com", Password = "123456" });
        result.ShouldNotHaveAnyValidationErrors();
    }
}