using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindFriend.Business.Exceptions;
using FindFriend.Business.Interfaces;
using FindFriend.Data.Interfaces;

public static class Roles
{
    public const string User = "user";
    public const string Admin = "admin";
}

namespace FindFriend.Business.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _data;

        public UserService(IUnitOfWork data)
        {
            _data = data;
        }

        public async Task<IEnumerable<(int Id, string Name, string Role)>> GetAllAsync()
        {
            var users = await _data.UserRepository.GetAllAsync();

            return users.Select(u => (u.Id, u.Name, u.Role));
        }

        public async Task<(int Id, string Name, string Role, 
            IEnumerable<(int Id, string Title, decimal Price)> Authored, 
            IEnumerable<(int Id, string Title, decimal Price)> Liked)> GetOneAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var user = await _data.UserRepository.GetOneAsync(u => u.Id == id);

            if (user is null) throw new ArgumentNullException(nameof(user));

            return (user.Id, user.Name, user.Role,
                user.Adds.Where(a => a.AuthorId == user.Id).Select(a => (a.Id, a.Title, a.Price)),
                user.Favourites.Where(a => a.Likers.Any(u => u.Id == user.Id)).Select(a => (a.Id, a.Title, a.Price)));
        }

        public async Task AddAdminRoleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            
            var user = await _data.UserRepository.GetByIdAsync(id);

            if (user is null) throw new ArgumentNullException(nameof(user));

            if (user.Role == Roles.Admin) throw new RoleException("This user is already admin");

            user.Role = Roles.Admin;
            
            _data.UserRepository.Update(user);
            await _data.SaveAsync();
        }

        public async Task DeleteAdminRoleAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            
            var user = await _data.UserRepository.GetByIdAsync(id);

            if (user is null) throw new ArgumentNullException(nameof(user));

            if (user.Role == Roles.User) throw new RoleException("This user is already not admin");

            user.Role = Roles.User;
            
            _data.UserRepository.Update(user);
            await _data.SaveAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            
            var user = await _data.UserRepository.GetByIdAsync(id);
            
            if (user is null) throw new ArgumentNullException(nameof(user));

            _data.UserRepository.Delete(id);
            await _data.SaveAsync();
        }
    }
}