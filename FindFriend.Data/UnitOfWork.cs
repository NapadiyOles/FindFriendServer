using System;
using System.Threading.Tasks;
using FindFriend.Data.Interfaces;
using FindFriend.Data.Repositories;

namespace FindFriend.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed;
        private readonly DataContext _data;
        private AddRepository _addRepository;
        private UserRepository _userRepository;
        private PictureRepository _pictureRepository;

        public UnitOfWork(DataContext data)
        {
            _disposed = false;
            _data = data;
        }

        public AddRepository AddRepository => _addRepository ??= new AddRepository(_data);
        public UserRepository UserRepository => _userRepository ??= new UserRepository(_data);
        public PictureRepository PictureRepository => _pictureRepository ??= new PictureRepository();

        public async Task SaveAsync() => await _data.SaveChangesAsync();
        
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _data.Dispose();
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}