using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FindFriend.Data.Entities;

namespace FindFriend.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        
    }
}