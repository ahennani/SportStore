using SportStore.Models;
using SportStore.Models.Dtos;
using SportStore.Models.Results;
using System.Threading.Tasks;

namespace SportStore.Managers.Repositories
{
    public interface IAuthenticationManager
    {
        Task<AuthResult> UserLoginAsync(UserLoginDTO loginDTO);
        Task<AuthResult> RegistrationAsync(UserAddDTO userDTO);
    }
}


