using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using FindFriend.Business.Models;

namespace FindFriend.Business.Interfaces
{
    public interface IAddService
    {
        public Task<IEnumerable<(int Id, string Tile, decimal Price, bool Liked)>> GetAllAsync(string name);
        public Task<IEnumerable<(int Id, string Tile, decimal Price, int Likes)>> GetAllByAuthorAsync(string name);
        public Task<IEnumerable<(int Id, string Tile, decimal Price)>> GetAllLikedAsync(string name);

        public Task<(int Id, string Title, string Description, decimal Price, string Phone,
            (int Id, string Name) Author)> GetOneAsync(int addId);

        public Task<Image> GetPicAsync(int addId);
        public Task AddAsync(AddDTO model, string name);
        public Task UpdateAsync(AddDTO model, string name);
        public Task DeleteAsync(int addId, string name);
        public Task LikeAsync(int addId, string name);
    }
}