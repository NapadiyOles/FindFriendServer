using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindFriend.Business.Exceptions;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Models;
using FindFriend.Data.Interfaces;

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

        public async Task DeleteUser(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));

            var user = await _data.UserRepository.GetByIdAsync(id);

            if (user is null) throw new ArgumentNullException(nameof(user));

            _data.UserRepository.Delete(id);
            await _data.SaveAsync();
        }
    }
}