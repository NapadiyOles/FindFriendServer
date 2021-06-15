using System.Collections.Generic;
using System.Threading.Tasks;

namespace FindFriend.Business.Interfaces
{
    public interface IUserService
    {
        public Task<IEnumerable<(int Id, string Name, string Role)>> GetAllAsync();

        public Task<(int Id, string Name, string Role,
            IEnumerable<(int Id, string Title, decimal Price)> Authored,
            IEnumerable<(int Id, string Title, decimal Price)> Liked)> GetOneAsync(int id);
        public Task AddAdminRoleAsync(int id);
        public Task DeleteAdminRoleAsync(int id);
        public Task DeleteUserAsync(int id);
    }
}