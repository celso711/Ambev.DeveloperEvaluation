using Ambev.DeveloperEvaluation.Application.Sale.CreateSale;
using Ambev.DeveloperEvaluation.Application.Users.CreateUser;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sale.CreateSale
{
    /// <summary>
    /// Handler for processing CreateSaleCommand requests
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of CreateSaleHandler
        /// </summary>
        /// <param name="saleRepository">The sale repository</param>
        /// <param name="mapper">The AutoMapper instance</param>
        public CreateSaleHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the CreateSaleCommand request
        /// </summary>
        /// <param name="command">The CreateSale command</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale details</returns>
        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            var sale = _mapper.Map<Domain.Entities.Sale>(command);

            foreach (var item in sale.Items)
            {
                item.Discount = CalculateDiscount(item.Quantity, item.UnitPrice);
                item.TotalAmount = (item.UnitPrice * item.Quantity) - item.Discount;
            }

            sale.TotalAmount = sale.Items.Sum(i => i.TotalAmount);

            sale.SaleNumber = GenerateSaleNumber();
            sale.SaleDate = DateTime.UtcNow;

            var createdSale = await _saleRepository.CreateAsync(sale, cancellationToken);
            var result = _mapper.Map<CreateSaleResult>(createdSale);
            return result;
        }

        private string GenerateSaleNumber()
        {
            return $"SALE-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
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
    }
}
