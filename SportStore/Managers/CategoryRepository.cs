namespace SportStore.Managers;

public class CategoryRepository : IStoreRepository<Category>
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> SaveChangesAsync()
        => await _context.SaveChangesAsync() > 0;

    public Task<IQueryable<Category>> GetAllAsync()
        => Task.Run(() => _context.Categories.AsNoTracking());

    public async Task<Category> GetByIdAsync(Guid id)
        => await _context.Categories.Where(c => c.CategoryId == id)
                                    .SingleOrDefaultAsync();

    public async Task<Category> AddAsync(Category category)
    {
        if (category is null)
            return null;

        var entity = await _context.Categories.AddAsync(category);

        return (await SaveChangesAsync()) ? entity.Entity : null;
    }

    public async Task<Category> DeleteAsync(Guid id)
    {
        var category = await GetByIdAsync(id);
        if (category is null)
            return null;

        var entity = _context.Categories.Remove(category);
        var result = await SaveChangesAsync();

        return result ? entity.Entity : null;
    }

    public Task<bool> DeleteRangeAsync(List<Category> categories)
    {
        _context.Categories.RemoveRange(categories);

        return SaveChangesAsync();
    }

    public async Task<Category> UpdateAsync(Category category)
    {
        if (category is null)
            return null;

        _context.Entry(category).State = EntityState.Modified;

        return await SaveChangesAsync() ? category : null;
    }
}
