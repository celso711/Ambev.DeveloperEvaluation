using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using FluentValidation.TestHelper;
using Xunit;

public class CreateSaleCommandValidatorTests
{
    private readonly CreateSaleCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_CustomerName_Is_Empty()
    {
        var command = new CreateSaleCommand { CustomerName = "" };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CustomerName);
    }
}
