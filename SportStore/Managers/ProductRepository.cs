using Microsoft.EntityFrameworkCore;
using SportStore.Data;
using SportStore.Managers.Repositories;
using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Managers
{
    public class ProductRepository : IStoreRepository<Product>
    {
        private readonly AppDbContext _dbContext;

        public ProductRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> SaveChangesAsync() => await _dbContext.SaveChangesAsync() > 0;

        public async Task<Product> AddAsync(Product product)
        {
            var entity = await _dbContext.AddAsync(product);
            var result = await SaveChangesAsync();
            return result ? entity.Entity : null;
        }

        public async Task<Product> GetByIdAsync(Guid id)
        {
            var product = await _dbContext.Products.Include(p => p.Category)
                                                   .Where(p => p.ProductId == id)
                                                   .FirstOrDefaultAsync();
            if (product is null)
                return null;

            _dbContext.Entry(product).State = EntityState.Detached;
            return product;
        }

        public async Task<Product> DeleteAsync(Guid id)
        {
            var product = await GetByIdAsync(id);
            if (product is null)
                return null;

            var entity = _dbContext.Products.Remove(product);
            var result = await SaveChangesAsync();

            return result ? entity.Entity : null;
        }

        public Task<bool> DeleteRangeAsync(List<Product> products)
        {
            _dbContext.Products.RemoveRange(products);
            var result = SaveChangesAsync();

            return result;
        }

        public async Task<Product> UpdateAsync(Guid id, Product product)
        {
            if (product is null)
                return null;

            _dbContext.Entry(product).State = EntityState.Modified;

            var result = await SaveChangesAsync();

            return result ? product : null;
        }

        public Task<IQueryable<Product>> ListAsync() => Task.Run(() => _dbContext.Products
                                                                                 .Include(p => p.Category)
                                                                                 .AsNoTracking()
                                                                                 .AsQueryable());

    }
}
