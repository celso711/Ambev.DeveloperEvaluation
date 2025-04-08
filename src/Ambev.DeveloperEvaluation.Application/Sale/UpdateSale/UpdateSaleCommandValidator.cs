using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleCommandValidator : AbstractValidator<UpdateSaleCommand>
    {
        public UpdateSaleCommandValidator()
        {
            RuleFor(sale => sale.CustomerId).NotEmpty();
            RuleFor(sale => sale.CustomerName).NotEmpty().Length(3, 100);
            RuleFor(sale => sale.BranchId).NotEmpty();
            RuleFor(sale => sale.BranchName).NotEmpty().Length(3, 100);
            RuleFor(sale => sale.Items).NotEmpty().WithMessage("A sale must have at least one item.");
            RuleForEach(sale => sale.Items).SetValidator(new UpdateSaleItemValidator());
        }
    }

    public class UpdateSaleItemValidator : AbstractValidator<UpdateSaleItemCommand>
    {
        public UpdateSaleItemValidator()
        {
            RuleFor(item => item.ProductId).NotEmpty();
            RuleFor(item => item.ProductName).NotEmpty();
            RuleFor(item => item.Quantity).InclusiveBetween(1, 20);
            RuleFor(item => item.UnitPrice).GreaterThan(0);
        }
    }
}
