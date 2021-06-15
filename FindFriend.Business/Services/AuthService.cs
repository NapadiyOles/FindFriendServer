using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FindFriend.Business.Exceptions;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Models;
using FindFriend.Data.Entities;
using FindFriend.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FindFriend.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _data;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork data, IConfiguration config)
        {
            _data = data;
            _config = config;

            AddDefaultAdmin().Wait();
        }
        
        public async Task RegisterAsync(UserDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Value can't be empty", nameof(model.Name));

            if (await _data.UserRepository.GetOneAsync(u => u.Name == model.Name) is not null)
                throw new AuthException("User with this name already exists");

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password?.Length < 5)
                throw new AuthException("Password can't be empty and be less then 5 characters long");

            var user = new User {Name = model.Name, Password = Encrypt(model.Password), Role = Roles.User};

            await _data.UserRepository.AddAsync(user);
            await _data.SaveAsync();
        }

        private string Encrypt(string password)
        {
            var data = Encoding.Unicode.GetBytes(password);
            var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);
            return Convert.ToBase64String(encrypted);
        }

        private string Decrypt(string password)
        {
            var data = Convert.FromBase64String(password);
            var decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(decrypted);
        }
        
        public async Task<string> LogInAsync(UserDTO model)
        {
            if (model is null) throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Name))
                throw new ArgumentException("Value can't be empty", nameof(model.Name));

            var user = await _data.UserRepository.GetOneAsync(u => u.Name == model.Name);

            if (user is null) throw new AuthException("User with this name is not registered");

            if (model.Password != Decrypt(user.Password)) throw new AuthException("Password is not correct");
            
            var authClaims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Name),
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (ClaimTypes.Role, user.Role),
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var authSignInKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(authSignInKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task AddDefaultAdmin()
        {
            if (await _data.UserRepository.GetOneAsync(u => u.Role == Roles.Admin) is null)
            {
                await _data.UserRepository.AddAsync(new User
                    {Name = "admin", Password = Encrypt("admin"), Role = Roles.Admin});
                await _data.SaveAsync();
            }
        }
    }
}