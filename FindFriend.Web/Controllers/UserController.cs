using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FindFriend.Business.Interfaces;
using FindFriend.Web.Filters;
using FindFriend.Web.Models.Add;
using FindFriend.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindFriend.Web.Controllers
{
    [Produces("application/json")]
    [Route("users")]
    [ApiController]
    [UserExceptionFilter]
    [Authorize(Roles = Roles.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAll()
        {
            var users = await _service.GetAllAsync();
            var model = users.Select(u => new UserViewModel {Id = u.Id, Name = u.Name, Role = u.Role});
            return Ok(model);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserFullViewModel>> GetOne(int id)
        {
            var user = await _service.GetOneAsync(id);
            var model = new UserFullViewModel
            {
                Id = user.Id, Name = user.Name, Role = user.Role,
                Authored = user.Authored.Select(a => new AddViewModel {Id = a.Id, Title = a.Title, Price = a.Price}),
                Liked = user.Liked.Select(a => new AddViewModel {Id = a.Id, Title = a.Title, Price = a.Price}),
            };
            return Ok(model);
        }

        [HttpPut("create_admin/{id}")]
        public async Task<ActionResult> AddAdmin(int id)
        {
            await _service.AddAdminRoleAsync(id);
            return Ok();
        }
        
        [HttpPut("delete_admin/{id}")]
        public async Task<ActionResult> DeleteAdmin(int id)
        {
            await _service.DeleteAdminRoleAsync(id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _service.DeleteUserAsync(id);
            return Ok();
        }
    }
}