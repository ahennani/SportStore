using SportStore.Models.Entities;
using System.Threading.Tasks;

namespace SportStore.Managers.Repositories
{
    public interface IUserManager
    {
        Task<User> FindByNameAsync(string userName);
        Task<User> FindByEmailAsync(string email);
        Task<bool> CreateUserAsync(User user, string password);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<string> GenerateToken(User user);
    }
}