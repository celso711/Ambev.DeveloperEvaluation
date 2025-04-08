using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.UpdateSale
{
    public class UpdateSaleRequestValidator : AbstractValidator<UpdateSaleRequest>
    {
        /// <summary>
        /// Initializes a new instance of the CreateSaleRequestValidator with defined validation rules.
        /// </summary>
        /// <remarks>
        /// Validation rules include:
        /// - Id: Required
        /// - CustomerId: Required
        /// - CustomerName: Required, length between 3 and 100 characters
        /// - BranchId: Required
        /// - BranchName: Required, length between 3 and 100 characters
        /// - Items: At least one item required
        /// - Quantity: Must be between 1 and 20
        /// - UnitPrice: Must be greater than zero
        /// </remarks>
        public UpdateSaleRequestValidator()
        {
            RuleFor(sale => sale.CustomerId).NotEmpty();
            RuleFor(sale => sale.CustomerName).NotEmpty().Length(3, 100);
            RuleFor(sale => sale.BranchId).NotEmpty();
            RuleFor(sale => sale.BranchName).NotEmpty().Length(3, 100);
            RuleFor(sale => sale.Items).NotEmpty().WithMessage("A sale must have at least one item.");
            RuleForEach(sale => sale.Items).SetValidator(new UpdateSaleItemRequestValidator());
        }
    }
}
