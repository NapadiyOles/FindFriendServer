using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FindFriend.Data.Entities;
using FindFriend.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FindFriend.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _data;

        public UserRepository(DataContext data)
        {
            _data = data;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var users = _data.Users.AsQueryable();
            return await users.ToListAsync();
        }

        public async Task<IEnumerable<User>> GetManyAsync(Expression<Func<User, bool>> expression)
        {
            var users = _data.Users.Where(expression);
            return await users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            var user = await _data.Users.FindAsync(id);
            _data.Entry(user).State = EntityState.Detached;
            return user;
        }

        public async Task AddAsync(User entity)
        {
            await _data.Users.AddAsync(entity);
        }

        public void Update(User entity)
        {
            _data.Users.Update(entity);
        }

        public void Delete(int id)
        {
            _data.Users.Remove(new User {Id = id});
        }

        public async Task<User> GetOneAsync(Expression<Func<User, bool>> expression)
        {
            var user = await _data.Users
                .Include(u => u.Adds)
                .Include(u => u.Favourites)
                .ThenInclude(a => a.Likers)
                .FirstOrDefaultAsync(expression);
            return user;
        }
    }
}