using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories.TestData;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.InMemory;

namespace Ambev.DeveloperEvaluation.Unit.Infrastructure.Repositories
{
    /// <summary>
    /// Contains unit tests for the <see cref="SaleRepository"/> class.
    /// </summary>
    public class SaleRepositoryTests
    {
        private readonly DefaultContext _context;
        private readonly SaleRepository _repository;

        public SaleRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<DefaultContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DefaultContext(options);
            _repository = new SaleRepository(_context);
        }

        [Fact(DisplayName = "Given valid sale When adding sale Then it is persisted")]
        public async Task AddAsync_ValidSale_PersistsSale()
        {
            var sale = SaleRepositoryTestData.GenerateValidSale();

            await _repository.CreateAsync(sale, CancellationToken.None);
            await _context.SaveChangesAsync();

            var saved = await _repository.GetByIdAsync(sale.Id, CancellationToken.None);

            saved.Should().NotBeNull();
            saved.SaleNumber.Should().Be(sale.SaleNumber);
        }

    }
}
