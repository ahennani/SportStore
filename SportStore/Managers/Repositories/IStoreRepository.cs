using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Managers.Repositories
{
    public interface IStoreRepository<TEntity>
    {
        Task<bool> SaveChangesAsync();
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(Guid id);
        Task<TEntity> DeleteAsync(Guid id);
        Task<bool> DeleteRangeAsync(List<TEntity> entities);
        Task<TEntity> UpdateAsync(Guid id, TEntity entity);
        Task<IQueryable<TEntity>> ListAsync();
    }
}
