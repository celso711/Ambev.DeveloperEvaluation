using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.GetSale
{
    /// <summary>
    /// Response model for GetSale operation
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// The unique identifier of the sale
        /// </summary>
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

        /// <summary>
        /// Collection of sale items.
        /// </summary>
        public List<GetSaleItemResult> Items { get; set; } = new();
    }
    /// <summary>
    /// Represents a sale item with product details and discounts.
    /// </summary>
    public class GetSaleItemResult
    {
        /// <summary>
        /// The unique identifier of the item sale
        /// </summary>
        public Guid Id { get; set; }

        public Guid SaleId { get; set; }

        /// <summary>
        /// External identity for the product.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Denormalized product name.
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Discount applied to the product.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Total amount after discount.
        /// </summary>
        public decimal TotalAmount { get; set; }
    }

}
