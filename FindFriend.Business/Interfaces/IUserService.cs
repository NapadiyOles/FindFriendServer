using System.Collections.Generic;
using System.Threading.Tasks;
using FindFriend.Business.Models;

namespace FindFriend.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<(int Id, string Name, string Role)>> GetAllAsync();
        public Task AddAdminRoleAsync(int id);
        public Task DeleteAdminRoleAsync(int id);
        public Task DeleteUser(int id);
    }
}