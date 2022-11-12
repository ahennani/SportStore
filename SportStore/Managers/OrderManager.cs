namespace SportStore.Managers;

public class OrderManager : IOrderManager
{
    private readonly IStoreRepository<Product> _producteRepository;
    private readonly IStoreRepository<Order> _orderRepository;

    public OrderManager(IStoreRepository<Order> orderRepository, IStoreRepository<Product> producteRepository)
    {
        _orderRepository = orderRepository;
        _producteRepository = producteRepository;
    }

    public Task<IQueryable<Order>> GetAllAsync()
        => _orderRepository.GetAllAsync();

    public Task<Order> GetByIdAsync(Guid id)
        => _orderRepository.GetByIdAsync(id);

    public Task<Order> AddAsync(Order order)
        => _orderRepository.AddAsync(order);

    public Task<Order> DeleteAsync(Guid id)
        => _orderRepository.DeleteAsync(id);

    public async Task<bool> DeleteRangeAsync(List<Order> orders)
        => await _orderRepository.DeleteRangeAsync(orders);

    public Task<Order> UpdateAsync(Order order)
        => _orderRepository.UpdateAsync(order);

    public async Task<IEnumerable<Product>> GetOrderProducts(Order order)
    {
        if (order is null)
            return null;

        var orderProducts = await (await _producteRepository.GetAllAsync())
                                   .Where(p => p.OrderId == order.OrderId)
                                   .ToListAsync();

        return orderProducts;
    }

    //public Task<bool> AddProductsToOrder(Order order, Product product)
    //{
    //    if (order is null || product is null)
    //        return null;

    //    order.Products.Add(product);

    //    return _orderRepository.SaveChangesAsync();
    //}

    public Task<bool> AddProductsToOrder(Order order, params Product[] products)
    {
        if (order is null || products is null || products.Length == 0)
            return null;

        order.Products.AddRange(products);

        return _orderRepository.SaveChangesAsync();
    }

}
