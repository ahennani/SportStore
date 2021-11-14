using SportStore.Models.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Managers.Repositories
{
    public interface IUserManager
    {
        Task<User> FindByNameAsync(string userName);
        Task<User> FindByEmailAsync(string email);
        Task<User> FindByIdAsync(Guid id);
        Task<IQueryable<User>> GetUsers();
        Task<bool> CreateUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<string> GenerateToken(User user);
    }
}