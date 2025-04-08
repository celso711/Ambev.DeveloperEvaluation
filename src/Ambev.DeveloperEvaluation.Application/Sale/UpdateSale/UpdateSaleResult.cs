using System.ComponentModel.DataAnnotations;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Represents the response returned after successfully updating a new sale.
    /// </summary>
    /// <remarks>
    /// This response contains the unique identifier of the newly updated sale,
    /// which can be used for subsequent operations or reference.
    /// </remarks>
    public class UpdateSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the updated sale.
        /// </summary>
        /// <value>A GUID that uniquely identifies the created sale in the system.</value>
        public Guid Id { get; set; }

        /// <summary>
        /// Unique sale number.
        /// </summary>
        [Required]
        public string SaleNumber { get; set; } = string.Empty;

        /// <summary>
        /// Date when the sale was made.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// External identity for the customer.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Denormalized customer name.
        /// </summary>
        public string CustomerName { get; set; } = string.Empty;

        /// <summary>
        /// External identity for the branch.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Denormalized branch name.
        /// </summary>
        public string BranchName { get; set; } = string.Empty;

        /// <summary>
        /// Total sale amount.
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Sale cancellation status.
        /// </summary>
        public bool IsCancelled { get; set; }
    }
}
