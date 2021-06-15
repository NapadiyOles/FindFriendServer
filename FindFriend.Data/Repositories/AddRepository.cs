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
    public class AddRepository : IAddRepository
    {
        private readonly DataContext _data;

        public AddRepository(DataContext data)
        {
            _data = data;
        }

        public async Task<IEnumerable<Add>> GetAllAsync()
        {
            var adds = _data.Adds.AsQueryable()
                .Include(a => a.Author)
                .Include(a => a.Likers);
            return await adds.ToListAsync();
        }

        public async Task<IEnumerable<Add>> GetManyAsync(Expression<Func<Add, bool>> expression)
        {
            var adds = _data.Adds.Where(expression)
                .Include(a => a.Author)
                .Include(a => a.Likers);
            return await adds.ToListAsync();
        }

        public async Task<Add> GetOneAsync(Expression<Func<Add, bool>> expression)
        {
            var add = await _data.Adds
                .Include(a => a.Author)
                .Include(a => a.Likers)
                .FirstOrDefaultAsync(expression);
            return add;
        }

        public async Task<Add> GetByIdAsync(int id)
        {
            var add = await _data.Adds.FindAsync(id);
            _data.Entry(add).State = EntityState.Detached;
            return add;
        }

        public async Task AddAsync(Add entity)
        {
            await _data.Adds.AddAsync(entity);
        }

        public void Update(Add entity)
        {
            _data.Adds.Update(entity);
        }

        public void Delete(int id)
        {
            _data.Adds.Remove(new Add {Id = id});
        }
    }
}