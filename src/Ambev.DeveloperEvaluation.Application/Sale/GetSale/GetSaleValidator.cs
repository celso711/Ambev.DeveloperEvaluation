using Ambev.DeveloperEvaluation.Application.Users.GetUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale
{
    /// <summary>
    /// Validator for GetSaleCommand
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for GetUserCommand
        /// </summary>
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}
