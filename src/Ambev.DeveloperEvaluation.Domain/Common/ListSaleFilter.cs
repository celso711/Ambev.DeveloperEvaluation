namespace Ambev.DeveloperEvaluation.Domain.Common
{
    public class ListSaleFilter
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? BranchId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
