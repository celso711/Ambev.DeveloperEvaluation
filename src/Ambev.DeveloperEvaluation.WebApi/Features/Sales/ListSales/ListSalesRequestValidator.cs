using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.ListSales
{
    public class ListSalesRequestValidator : AbstractValidator<ListSaleRequest>
    {
        /// <summary>
        /// Initializes validation rules for ListSalesRequest
        /// </summary>
        public ListSalesRequestValidator()
        {
            RuleFor(x => x.Page).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.EndDate).GreaterThanOrEqualTo(x => x.StartDate).When(x => x.StartDate.HasValue);
        }
    }
}
