using System;
using System.Threading.Tasks;
using FindFriend.Data.Repositories;

namespace FindFriend.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public AddRepository AddRepository { get; }
        public UserRepository UserRepository { get; }
        public PictureRepository PictureRepository { get; }
        public Task SaveAsync();
    }
}