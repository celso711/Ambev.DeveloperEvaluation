using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSale
{
    public class SaleItemValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public SaleItemValidator()
        {
            RuleFor(item => item.ProductId).NotEmpty();
            RuleFor(item => item.ProductName).NotEmpty();
            RuleFor(item => item.Quantity).InclusiveBetween(1, 20);
            RuleFor(item => item.UnitPrice).GreaterThan(0);
        }
    }
}
