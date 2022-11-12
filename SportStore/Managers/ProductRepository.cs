namespace SportStore.Managers;

public class ProductRepository : IStoreRepository<Product>
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> SaveChangesAsync()
        => await _dbContext.SaveChangesAsync() > 0;

    public Task<IQueryable<Product>> GetAllAsync()
        => Task.Run(() => _dbContext.Products.Include(p => p.Category).AsQueryable());

    public Task<Product> GetByIdAsync(Guid id)
        => _dbContext.Products.Include(p => p.Category)
                              .Where(p => p.ProductId == id)
                              .SingleOrDefaultAsync();

    public async Task<Product> AddAsync(Product product)
    {
        if (product is null)
            return null;

        var entity = await _dbContext.AddAsync(product);

        return (await SaveChangesAsync()) ? entity.Entity : null;
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

        return SaveChangesAsync();
    }

    public async Task<Product> UpdateAsync(Product product)
    {
        if (product is null)
            return null;

        var entity = _dbContext.Products.Update(product);

        return await SaveChangesAsync() ? entity.Entity : null;
    }
}
