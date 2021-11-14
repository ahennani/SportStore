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
    public class OrderRepository : IStoreRepository<Order>
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IQueryable<Order>> GetAllAsync()
            => Task.Run(() => _dbContext.Orders.AsNoTracking());

        public Task<Order> GetByIdAsync(Guid id)
            => _dbContext.Orders.Where(o => o.OrderId == id)
                                .SingleOrDefaultAsync();

        public async Task<Order> AddAsync(Order order)
        {
            if (order is null)
                return null;

            var entity = await _dbContext.Orders.AddAsync(order);

            return await SaveChangesAsync() ? entity.Entity : null;
        }

        public async Task<Order> DeleteAsync(Guid id)
        {
            var order = await GetByIdAsync(id);
            if (order is null)
                return null;

            var entity = _dbContext.Orders.Remove(order);

            return await SaveChangesAsync() ? entity.Entity : null;
        }

        public async Task<bool> DeleteRangeAsync(List<Order> orders)
        {
            if (orders is null || orders.Count == 0)
                return false;

            _dbContext.Orders.RemoveRange(orders);

            return await SaveChangesAsync();
        }

        public async Task<Order> UpdateAsync(Order order)
        {
            if (order is null)
                return null;

            var entity = _dbContext.Orders.Update(order);

            return await SaveChangesAsync() ? entity.Entity : null;
        }

        public async Task<bool> SaveChangesAsync()
            => await _dbContext.SaveChangesAsync() > 0;

    }
}
