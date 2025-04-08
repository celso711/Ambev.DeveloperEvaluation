using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.ListSale
{
    public class ListSaleValidator : AbstractValidator<ListSaleCommand>
    {
        /// <summary>
        /// Initializes validation rules for ListSalesRequest
        /// </summary>
        public ListSaleValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.StartDate.HasValue);
        }
    }
}
