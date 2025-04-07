using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(sale => sale.CustomerId)
                .NotEmpty();
            RuleFor(sale => sale.CustomerName)
                .NotEmpty().Length(3, 100);
            RuleFor(sale => sale.BranchId)
                .NotEmpty();
            RuleFor(sale => sale.BranchName)
                .NotEmpty().Length(3, 100);
            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("A sale must have at least one item.");
            RuleForEach(sale => sale.Items).
                SetValidator(new SaleItemValidator());
        }
    }

    public class SaleItemValidator : AbstractValidator<SaleItemCommand>
    {
        public SaleItemValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty();
            RuleFor(item => item.ProductName)
                .NotEmpty();
            RuleFor(item => item.Quantity)
                .InclusiveBetween(1, 20);
            RuleFor(item => item.UnitPrice)
                .GreaterThan(0);
        }
    }
}
