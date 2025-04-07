using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of SaleRepository
        /// </summary>
        /// <param name="context">The database context</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the database
        /// </summary>
        /// <param name="sale">The sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale</returns>
        public async Task<Sale> CreateAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _context.Sale.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return sale;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            var sale = await GetByIdAsync(id, cancellationToken);
            if (sale == null)
                return false;

            _context.Sale.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Sale.AsNoTracking().FirstOrDefaultAsync(o => o.Id == id, cancellationToken);
        }

        public async Task<Sale> UpdateAsync(Guid id, Sale sale, CancellationToken cancellationToken)
        {
            var existingSale = await GetByIdAsync(id, cancellationToken);

            if (existingSale == null)
            {
                throw new KeyNotFoundException($"Sale with ID {id} not found.");
            }

            _context.Entry(existingSale).CurrentValues.SetValues(sale);
            _context.Entry(existingSale).State = EntityState.Modified;

            await _context.SaveChangesAsync(cancellationToken);

            return existingSale;
        }

    }
}
