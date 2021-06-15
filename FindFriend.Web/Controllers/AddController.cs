using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Models;
using FindFriend.Web.Filters;
using FindFriend.Web.Models.Add;
using FindFriend.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FindFriend.Web.Controllers
{
    [Produces("application/json")]
    [Route("adds")]
    [ApiController]
    [AddExceptionFilter]
    [Authorize]
    public class AddController : ControllerBase
    {
        private readonly IAddService _service;

        public AddController(IAddService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AddLikedViewModel>>> GetAll()
        {
            var name = User.Claims.ElementAt(0).Value;
            var adds = await _service.GetAllAsync(name);
            return Ok(adds.Select(a => new AddLikedViewModel
                {Id = a.Id, Title = a.Tile, Price = a.Price, Liked = a.Liked}));
        }

        [HttpGet("authored")]
        public async Task<ActionResult<IEnumerable<AddLikesViewModel>>> GetAllAuthored()
        {
            var name = User.Claims.ElementAt(0).Value;
            var adds = await _service.GetAllByAuthorAsync(name);
            return Ok(adds.Select(a => new AddLikesViewModel
                {Id = a.Id, Title = a.Tile, Price = a.Price, Likes = a.Likes}));
        }

        [HttpGet("liked")]
        public async Task<ActionResult<IEnumerable<AddViewModel>>> GetAllLiked()
        {
            var name = User.Claims.ElementAt(0).Value;
            var adds = await _service.GetAllLikedAsync(name);
            return Ok(adds.Select(a => new AddViewModel
                {Id = a.Id, Title = a.Tile, Price = a.Price}));
        }

        [HttpGet("{id}")]
        public async
            Task<ActionResult<AddFullViewModel>> GetOne(int id)
        {
            var add = await _service.GetOneAsync(id);
            var model = new AddFullViewModel
            {
                Id = add.Id, Title = add.Title, Price = add.Price, 
                Description = add.Description, Phone = add.Phone,
                Author = new AuthorViewModel {Id = add.Author.Id, Name = add.Author.Name}
            };
            return Ok(model);
        }

        [HttpGet("picture/{id}"), DisableRequestSizeLimit]
        public async Task<FileResult> GetPic(int id)
        {
            byte[] picContent;
            var pic = await _service.GetPicAsync(id);

            using (var stream = new MemoryStream())
            {
                pic.Save(stream, ImageFormat.Jpeg);
                picContent = stream.ToArray();
            }

            return File(picContent, "image/jpeg");
        }

        [HttpPost]
        public async Task<ActionResult> AddAdd([FromForm] AddCreateModel add)
        {
            var name = User.Claims.ElementAt(0).Value;
            using (var stream = new MemoryStream())
            {
                await add.Picture.CopyToAsync(stream);

                await _service.AddAsync(new AddDTO
                {
                    Title = add.Title,
                    Description = add.Description,
                    Price = add.Price,
                    Phone = add.Phone,
                    Picture = new Bitmap(stream)
                }, name);
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ChangeAdd(int id, [FromForm] AddChangeModel add)
        {
            var name = User.Claims.ElementAt(0).Value;
            await _service.UpdateAsync(new AddDTO
            {
                Id = id,
                Title = add.Title,
                Description = add.Description,
                Phone = add.Phone,
                Price = add.Price
            }, name);

            return Ok();
        }

        [HttpPut("like/{id}")]
        public async Task<ActionResult> LikeAdd(int id)
        {
            var name = User.Claims.ElementAt(0).Value;
            await _service.LikeAsync(id, name);
            return Ok();
        }
        
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAdd(int id)
        {
            var name = User.Claims.ElementAt(0).Value;
            await _service.DeleteAsync(id, name);
            return Ok();
        }
    }
}