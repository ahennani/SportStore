using AutoMapper;
using SportStore.Managers.Repositories;
using SportStore.Models;
using SportStore.Models.Dtos;
using SportStore.Models.Entities;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using SportStore.Models.Results;

namespace SportStore.Managers
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly IUserManager _userManager;
        private readonly IMapper _mapper;

        public AuthenticationManager(IUserManager userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<AuthResult> UserLoginAsync(UserLoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.Username);

            if (user is null || await _userManager.CheckPasswordAsync(user, loginDTO.Password) is false)
            {
                return new AuthResult { ErrorMessage = $"Username or Password Incorrect !!." };
            }

            // Generate JWT Token
            var token = await GetToken(user);

            return new AuthResult
            {
                Email = user.Email,
                Username = user.UserName,
                Token = token
            };
        }

        public async Task<AuthResult> RegistrationAsync(UserAddDTO userDTO)
        {
            var result = new AuthResult();
            if (await _userManager.FindByNameAsync(userDTO.Username) is not null)
            {
                return new AuthResult { ErrorMessage = $"User with Username: {userDTO.Username} Already Exists !!." };
            }

            if (await _userManager.FindByEmailAsync(userDTO.Email) is not null)
            {
                return new AuthResult { ErrorMessage = $"User with Email: {userDTO.Email} Already Exists !!." };
            }

            var user = _mapper.Map<User>(userDTO);

            var isCreated = await _userManager.CreateUserAsync(user, userDTO.Password);

            if (isCreated)
            {
                result.Email = user.Email;
                result.Username = user.UserName;
                result.Token = await _userManager.GenerateToken(user);
                return result;
            }

            result.ErrorMessage = "Something Went Wrong. Try again later or Contact Us !!";
            return result;
        }

        private async Task<string> GetToken(User user)
        {
            return await _userManager.GenerateToken(user);
        }
    }
}