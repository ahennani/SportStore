namespace SportStore.Managers.Repositories;

public interface IAuthenticationManager
{
    Task<AuthResult> UserLoginAsync(UserLoginDTO loginDTO);
    Task<AuthResult> RegistrationAsync(UserAddDTO userDTO);
}


