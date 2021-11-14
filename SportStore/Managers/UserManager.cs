using SportStore.Models.Entities;
using SportStore.Data;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SportStore.Managers.Repositories;

namespace SportStore.Managers
{
    public class UserManager : IUserManager
    {
        private readonly AppDbContext _dbcontext;
        private readonly IConfiguration _configuration;

        public UserManager(AppDbContext dbcontext, IConfiguration configuration)
        {
            _dbcontext = dbcontext;
            _configuration = configuration;
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return _dbcontext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public Task<User> FindByEmailAsync(string email)
        {
            return _dbcontext.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public Task<User> FindByIdAsync(Guid id)
        {
            return _dbcontext.Users.SingleOrDefaultAsync(u => u.UserId == id);
        }

        public Task<IQueryable<User>> GetUsers()
        {
            return Task.Run(() => _dbcontext.Users.AsNoTracking());
        }

        public async Task<bool> CreateUserAsync(User user, string password)
        {
            if (user is null)
            {
                return false;
            }

            GererateHash(password, out byte[] hash, out byte[] salt);
            user.Passwordhash = hash;
            user.Passwordsalt = salt;

            _dbcontext.Users.Add(user);

            return await _dbcontext.SaveChangesAsync() > 0;
        }

        public Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (user is null || ValidateHash(password, user.Passwordhash, user.Passwordsalt) is false)
            {
                return Task.Run(() => false);
            }

            return Task.Run(() => true);
        }

        public async Task<string> GenerateToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.GetSection("secret").Value));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = await GetClaims(user);
            
			if (user.UserName is "user01")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("Owner", "Yes"));
            }
            if (user.UserName is "user02")
            {
                claims.Add(new Claim("ClaimKey", "ClaimValue"));
                claims.Add(new Claim("Key", "Value"));
            }
            if (user.UserName is "user03")
            {
                claims.Add(new Claim("Key", "Value"));
            }

            var Desc = new SecurityTokenDescriptor
            {
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = signingCredentials,
                Subject = new ClaimsIdentity(claims)//,
                //Audience = jwtSettings.GetSection("validAudience").Value,
                //Issuer = jwtSettings.GetSection("validIssuer").Value
            };
            var handler = new JwtSecurityTokenHandler();
            var token = handler.CreateToken(Desc);

            return handler.WriteToken(token);

            //var tokenD = new JwtSecurityToken(
            //   issuer: jwtSettings.GetSection("validIssuer").Value,
            //   audience: jwtSettings.GetSection("validAudience").Value,
            //   claims: claims,
            //   expires: DateTime.Now.AddMinutes(60),
            //   signingCredentials: signingCredentials);

            //return new JwtSecurityTokenHandler().WriteToken(tokenD);
        }

        #region Private Fields

        private static void GererateHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using var hash = new System.Security.Cryptography.HMACSHA512();
            PasswordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(Password));
            PasswordSalt = hash.Key;
        }

        private static bool ValidateHash(string password, byte[] passwordhash, byte[] passwordsalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512(passwordsalt))
            {
                var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < newPassHash.Length; i++)
                {
                    if (newPassHash[i] != passwordhash[i])
                        return false;
                }
            }
            return true;
        }

        private static Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>() {
                                               new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                                               new Claim(JwtRegisteredClaimNames.Email, user.Email),
                                               new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                                               new Claim("UserId", user.UserId.ToString()),
                                               new Claim(ClaimTypes.Role, "User")
                                           };

            return Task.Run(() => claims);
        }

        #endregion

    }
}
