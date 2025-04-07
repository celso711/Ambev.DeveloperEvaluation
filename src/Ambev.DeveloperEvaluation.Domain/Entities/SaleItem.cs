using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sale item with product details and discounts.
    /// </summary>
    public class SaleItem : BaseEntity
    {

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
