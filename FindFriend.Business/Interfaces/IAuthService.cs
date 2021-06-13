using System.Threading.Tasks;
using FindFriend.Business.Models;

namespace FindFriend.Business.Interfaces
{
    public interface IAuthService
    {
        Task RegisterAsync(UserDTO model);
        Task<string> LogInAsync(UserDTO model);
    }
}