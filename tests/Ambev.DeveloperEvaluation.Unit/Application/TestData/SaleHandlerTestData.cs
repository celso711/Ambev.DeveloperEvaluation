using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.Unit.Application.Sale;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Application.TestData
{
    /// <summary>
    /// Provides test data for <see cref="CreateSaleHandlerTests"/>, <see cref="GetSaleHandlerTests"/>, <see cref="UpdateSaleHandlerTests"/>, and <see cref="DeleteSaleHandlerTests"/>.
    /// </summary>
    public static class SaleHandlerTestData
    {
        //// <summary>
        /// Faker instance to generate valid <see cref="CreateSaleCommand"/> objects with randomized data.
        /// </summary>
        private static readonly Faker<CreateSaleCommand> createSaleCommandFaker = new Faker<CreateSaleCommand>()
            .RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString())
            .RuleFor(s => s.CustomerName, f => f.Name.FullName())
            .RuleFor(s => s.BranchId, f => f.Random.Guid().ToString())
            .RuleFor(s => s.BranchName, f => f.Address.City())
            .RuleFor(s => s.Items, f => GenerateSaleItemCommands(f.Random.Number(1, 5)));

        /// <summary>
        /// Faker instance to generate valid <see cref="UpdateSaleCommand"/> objects with randomized data.
        /// </summary>
        private static readonly Faker<UpdateSaleCommand> updateSaleCommandFaker = new Faker<UpdateSaleCommand>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.CustomerId, f => f.Random.Guid().ToString())
            .RuleFor(s => s.CustomerName, f => f.Name.FullName())
            .RuleFor(s => s.BranchId, f => f.Random.Guid().ToString())
            .RuleFor(s => s.BranchName, f => f.Address.City())
            .RuleFor(s => s.Items, f => GenerateUpdateSaleItemCommands(f.Random.Number(1, 5)));

        /// <summary>
        /// Faker instance to generate <see cref="Sale"/> entities with randomized data.
        /// </summary>
        private static readonly Faker<DeveloperEvaluation.Domain.Entities.Sale> saleFaker 
            = new Faker<DeveloperEvaluation.Domain.Entities.Sale>()
            .RuleFor(s => s.Id, f => f.Random.Guid())
            .RuleFor(s => s.CustomerId, f => f.Random.Guid())
            .RuleFor(s => s.CustomerName, f => f.Name.FullName())
            .RuleFor(s => s.BranchId, f => f.Random.Guid())
            .RuleFor(s => s.BranchName, f => f.Address.City())
            .RuleFor(s => s.SaleDate, f => f.Date.Past(1))
            .RuleFor(s => s.Items, f => GenerateSaleItems(f.Random.Number(1, 5)))
            .RuleFor(s => s.TotalAmount, (f, s) => CalculateTotalAmount(s.Items));

        /// <summary>
        /// Generates a valid <see cref="CreateSaleCommand"/> with randomized data.
        /// </summary>
        public static CreateSaleCommand GenerateValidCreateCommand() => createSaleCommandFaker.Generate();

        /// <summary>
        /// Generates a valid <see cref="UpdateSaleCommand"/> with randomized data.
        /// </summary>
        public static UpdateSaleCommand GenerateValidUpdateCommand() => updateSaleCommandFaker.Generate();

        /// <summary>
        /// Generates a valid <see cref="Sale"/> entity with randomized data.
        /// </summary>
        public static DeveloperEvaluation.Domain.Entities.Sale GenerateSale() => saleFaker.Generate();

        /// <summary>
        /// Generates and saves a <see cref="Sale"/> entity to the repository before deletion (DDD best practice).
        /// </summary>
        public static async Task<DeveloperEvaluation.Domain.Entities.Sale> GenerateAndSaveSaleAsync(ISaleRepository saleRepository, CancellationToken cancellationToken)
        {
            var createCommand = GenerateValidCreateCommand();
            var sale = MapToSale(createCommand);

            await saleRepository.CreateAsync(sale, cancellationToken);
            return sale;
        }

        /// <summary>
        /// Maps a <see cref="CreateSaleCommand"/> to a <see cref="Sale"/> aggregate root.
        /// </summary>
        private static DeveloperEvaluation.Domain.Entities.Sale MapToSale(CreateSaleCommand command)
        {
            var items = new List<SaleItem>();

            foreach (var itemCommand in command.Items)
            {
                var discount = CalculateDiscount(itemCommand.Quantity, itemCommand.UnitPrice);
                items.Add(new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = Guid.Parse(itemCommand.ProductId),
                    ProductName = itemCommand.ProductName,
                    Quantity = itemCommand.Quantity,
                    UnitPrice = itemCommand.UnitPrice,
                    Discount = discount,
                    TotalAmount = itemCommand.UnitPrice * itemCommand.Quantity - discount
                });
            }

            return new DeveloperEvaluation.Domain.Entities.Sale
            {
                Id = Guid.NewGuid(),
                CustomerId = Guid.Parse(command.CustomerId),
                CustomerName = command.CustomerName,
                BranchId = Guid.Parse(command.BranchId),
                BranchName = command.BranchName,
                SaleDate = DateTime.UtcNow,
                Items = items,
                TotalAmount = CalculateTotalAmount(items)
            };
        }

        /// <summary>
        /// Generates a valid <see cref="Sale"/> entity with the specified ID.
        /// </summary>
        /// <param name="saleId">The ID to assign to the generated sale.</param>
        public static DeveloperEvaluation.Domain.Entities.Sale GenerateSale(Guid saleId)
        {
            var sale = saleFaker.Generate();
            sale.Id = saleId;
            return sale;
        }

        /// <summary>
        /// Generates a list of <see cref="UpdateSaleItemCommand"/> with the specified count.
        /// </summary>
        private static List<UpdateSaleItemCommand> GenerateUpdateSaleItemCommands(int count)
        {
            var itemFaker = new Faker<UpdateSaleItemCommand>()
                .RuleFor(i => i.ProductId, f => f.Random.Guid())
                .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 500));

            return itemFaker.Generate(count);
        }

        /// <summary>
        /// Generates a list of <see cref="SaleItemCommand"/> with the specified count.
        /// </summary>
        private static List<SaleItemCommand> GenerateSaleItemCommands(int count)
        {
            var itemFaker = new Faker<SaleItemCommand>()
                .RuleFor(i => i.ProductId, f => f.Random.Guid().ToString())
                .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 500));

            return itemFaker.Generate(count);
        }

        /// <summary>
        /// Generates a list of <see cref="SaleItem"/> with the specified count.
        /// </summary>
        private static List<SaleItem> GenerateSaleItems(int count)
        {
            var itemFaker = new Faker<SaleItem>()
                .RuleFor(i => i.Id, f => f.Random.Guid())
                .RuleFor(i => i.ProductId, f => f.Random.Guid())
                .RuleFor(i => i.ProductName, f => f.Commerce.ProductName())
                .RuleFor(i => i.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(i => i.UnitPrice, f => f.Finance.Amount(10, 500))
                .RuleFor(i => i.Discount, (f, i) => CalculateDiscount(i.Quantity, i.UnitPrice))
                .RuleFor(i => i.TotalAmount, (f, i) => i.UnitPrice * i.Quantity - i.Discount);

            return itemFaker.Generate(count);
        }

        /// <summary>
        /// Calculates the discount based on quantity and unit price.
        /// </summary>
        private static decimal CalculateDiscount(int quantity, decimal unitPrice)
        {
            if (quantity > 20)
                throw new ArgumentException("Cannot sell more than 20 items of the same product.");

            if (quantity >= 10) return unitPrice * quantity * 0.20m;
            if (quantity >= 4) return unitPrice * quantity * 0.10m;

            return 0m;
        }

        /// <summary>
        /// Calculates the total amount for a list of <see cref="SaleItem"/>.
        /// </summary>
        private static decimal CalculateTotalAmount(IEnumerable<SaleItem> items)
        {
            decimal total = 0m;
            foreach (var item in items)
            {
                total += item.TotalAmount;
            }
            return total;
        }
    }
}
