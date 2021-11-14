using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Managers.Repositories
{
    public interface IOrderManager
    {
        Task<Order> AddAsync(Order order);
        Task<Order> DeleteAsync(Guid id);
        Task<bool> DeleteRangeAsync(List<Order> orders);
        Task<IQueryable<Order>> GetAllAsync();
        Task<Order> GetByIdAsync(Guid id);
        Task<Order> UpdateAsync(Order order);
        Task<IEnumerable<Product>> GetOrderProducts(Order order);
        //Task<bool> AddProductsToOrder(Order order, Product product);
        Task<bool> AddProductsToOrder(Order order, params Product[] products);
    }
}
