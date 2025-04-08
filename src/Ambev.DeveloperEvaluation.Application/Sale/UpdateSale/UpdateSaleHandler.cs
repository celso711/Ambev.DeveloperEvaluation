using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    /// <summary>
    /// Handler for processing GetSaleCommand requests
    /// </summary>
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of UpdateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the UpdateSaleCommand request
        /// </summary>
        /// <param name="command">The UpdateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The updated sale details</returns>
        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (existingSale == null)
                throw new KeyNotFoundException($"Sale with ID {command.Id} not found.");

            // Update sale metadata
            UpdateSaleMetadata(existingSale, command);

            // Update sale items (add new, update existing, remove missing)
            UpdateSaleItems(existingSale, command);

            // Recalculate total amounts and discounts
            RecalculateSaleTotals(existingSale);

            var createdSale = await _saleRepository.UpdateAsync(existingSale.Id, existingSale, cancellationToken);
            var result = _mapper.Map<UpdateSaleResult>(createdSale);
            return result;
        }


        /// <summary>
        /// Calculates the discount based on quantity and unit price.
        /// </summary>
        private decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 items of the same product.");

            if (quantity >= 10) return unitPrice * quantity * 0.20m;
            if (quantity >= 4) return unitPrice * quantity * 0.10m;

            return 0m;
        }

        /// <summary>
        /// Updates sale-level metadata (customer and branch info).
        /// </summary>
        private void UpdateSaleMetadata(Ambev.DeveloperEvaluation.Domain.Entities.Sale sale, UpdateSaleCommand command)
        {
            sale.CustomerId = Guid.Parse(command.CustomerId);
            sale.CustomerName = command.CustomerName;
            sale.BranchId = Guid.Parse(command.BranchId);
            sale.BranchName = command.BranchName;
        }

        /// <summary>
        /// Updates existing items, adds new ones, and removes items not in the update request.
        /// </summary>
        private void UpdateSaleItems(Ambev.DeveloperEvaluation.Domain.Entities.Sale sale, UpdateSaleCommand command)
        {
            var requestItems = command.Items.ToDictionary(i => i.ProductId);

            // Update or remove existing items
            sale.Items.RemoveAll(item =>
            {
                if (requestItems.TryGetValue(item.ProductId, out var reqItem))
                {
                    UpdateItem(item, reqItem);
                    requestItems.Remove(item.ProductId); // Item updated, remove from requestItems
                    return false; // Keep item
                }
                return true; // Remove item if not found in request
            });

            // Add new items
            sale.Items.AddRange(requestItems.Values.Select(reqItem => CreateItem(sale.Id, reqItem)));
        }

        /// <summary>
        /// Recalculates discounts and total amounts for all items and updates the sale's total amount.
        /// </summary>
        private void RecalculateSaleTotals(Ambev.DeveloperEvaluation.Domain.Entities.Sale sale)
        {
            foreach (var item in sale.Items)
            {
                item.Discount = CalculateDiscount(item.Quantity, item.UnitPrice);
                item.TotalAmount = (item.UnitPrice * item.Quantity) - item.Discount;
            }

            sale.TotalAmount = sale.Items.Sum(i => i.TotalAmount);
        }

        /// <summary>
        /// Updates an existing SaleItem with new data.
        /// </summary>
        private static void UpdateItem(SaleItem item, UpdateSaleItemCommand req)
        {
            item.ProductName = req.ProductName;
            item.Quantity = req.Quantity;
            item.UnitPrice = req.UnitPrice;
        }

        /// <summary>
        /// Creates a new SaleItem based on the request.
        /// </summary>
        private SaleItem CreateItem(Guid saleId, UpdateSaleItemCommand req) => new()
        {
            Id = Guid.NewGuid(),
            SaleId = saleId,
            ProductId = req.ProductId,
            ProductName = req.ProductName,
            Quantity = req.Quantity,
            UnitPrice = req.UnitPrice,
            Discount = CalculateDiscount(req.Quantity, req.UnitPrice),
            TotalAmount = (req.UnitPrice * req.Quantity) - CalculateDiscount(req.Quantity, req.UnitPrice)
        };
    }
}
