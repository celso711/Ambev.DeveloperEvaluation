using Ambev.DeveloperEvaluation.Application.Users.DeleteUser;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.DeleteSale
{
    /// <summary>
    /// Validator for DeleteSaleCommand
    /// </summary>
    public class DeleteSaleValidator : AbstractValidator<DeleteSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for DeleteSaleCommand
        /// </summary>
        public DeleteSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Sale ID is required");
        }
    }
}
