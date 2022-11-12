namespace SportStore.Managers.Repositories;

public interface IStoreRepository<TEntity>
{
    Task<bool> SaveChangesAsync();
    Task<TEntity> AddAsync(TEntity entity);
    Task<TEntity> GetByIdAsync(Guid id);
    Task<TEntity> DeleteAsync(Guid id);
    Task<bool> DeleteRangeAsync(List<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<IQueryable<TEntity>> GetAllAsync();
}
