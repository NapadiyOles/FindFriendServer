using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using FindFriend.Business.Exceptions;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Models;
using FindFriend.Data.Entities;
using FindFriend.Data.Interfaces;

namespace FindFriend.Business.Services
{
    public class AddService : IAddService
    {
        private readonly IUnitOfWork _data;
        private readonly IMapper _mapper;

        public AddService(IUnitOfWork data, IMapper mapper)
        {
            _data = data;
            _mapper = mapper;
        }


        public async Task<IEnumerable<(int Id, string Tile, decimal Price, bool Liked)>> GetAllAsync(string name)
        {
            var adds = await _data.AddRepository.GetAllAsync();

            return adds.Select(a => (a.Id, a.Title, a.Price, a.Likers.Any(u => u.Name == name)));
        }
        
        public async Task<IEnumerable<(int Id, string Tile, decimal Price, int Likes)>> GetAllByAuthorAsync(string name)
        {
            var adds = await _data.AddRepository.GetManyAsync(a => a.Author.Name == name);

            return adds.Select(a => (a.Id, a.Title, a.Price, a.Likers.Count()));
        }

        public async Task<IEnumerable<(int Id, string Tile, decimal Price)>> GetAllLikedAsync(string name)
        {
            var adds = await _data.AddRepository.GetManyAsync(a => a.Likers.Any(u => u.Name == name));

            return adds.Select(a => (a.Id, a.Title, a.Price));
        }

        public async Task<AddDTO> GetOneAsync(int addId)
        {
            var add = await _data.AddRepository.GetByIdAsync(addId);

            return _mapper.Map<AddDTO>(add);
        }

        public async Task AddAsync(AddDTO model, string name)
        {
            var user = await _data.UserRepository.GetOneAsync(u => u.Name == name);

            if (user is null) throw new ArgumentNullException(nameof(user));

            if (model is null) throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Title)) throw new ArgumentException("Title can't be empty");

            if (string.IsNullOrWhiteSpace(model.Description)) throw new ArgumentException("Description can't be empty");

            if (model.Price < 0) throw new ArgumentException("Price can't be less then zero");

            var regex = new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$");
            if (!regex.IsMatch(model.Phone)) throw new ArgumentException("Phone number is invalid");
            
            var add = _mapper.Map<Add>(model);
            add.AuthorId = user.Id;

            await _data.AddRepository.AddAsync(add);
            await _data.SaveAsync();

            if (model.Picture is not null)
            {
                _data.PictureRepository.SaveImage(model.Picture, Guid.NewGuid().ToString());
            }
        }
        
        public async Task UpdateAsync(AddDTO model, string name)
        {
            var user = await _data.UserRepository.GetOneAsync(u => u.Name == name);

            if (model.AuthorId == user.Id || user.Role == Roles.Admin)
            {
                var add = _mapper.Map<Add>(model);
                _data.AddRepository.Update(add);
                await _data.SaveAsync();
            }
            else throw new AccessException("Current user must be an author or admin");
        }
        
        public async Task DeleteAsync(int addId, string name)
        {
            var user = await _data.UserRepository.GetOneAsync(u => u.Name == name);
            var add = await _data.AddRepository.GetByIdAsync(addId);

            if (add is null) throw new ArgumentNullException(nameof(add));

            if (add.AuthorId == user.Id || user.Role == Roles.Admin)
            {
                _data.AddRepository.Delete(addId);
                await _data.SaveAsync();
            }
            else throw new AccessException("Current user must be an author or admin");
        }

        public async Task LikeAsync(int addId, string name)
        {
            var user = await _data.UserRepository.GetOneAsync(u => u.Name == name);
            var add = await _data.AddRepository.GetOneAsync(a => a.Id == addId);
            var likers = add.Likers.ToList();
            likers.Add(user);
            add.Likers = likers;
            
            _data.AddRepository.Update(add);
            await _data.SaveAsync();
        }
    }
}