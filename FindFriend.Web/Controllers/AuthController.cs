using System.Threading.Tasks;
using FindFriend.Business.Interfaces;
using FindFriend.Business.Models;
using FindFriend.Web.Filters;
using FindFriend.Web.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindFriend.Web.Controllers
{
    [Produces("application/json")]
    [Route("authentication")]
    [ApiController]
    [AuthExceptionFilter]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] UserLogInModel user)
        {
            await _service.RegisterAsync(new UserDTO {Name = user.Name, Password = user.Password});
            return Ok();
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> LogIn([FromForm] UserLogInModel user)
        {
            var token = await _service.LogInAsync(new UserDTO {Name = user.Name, Password = user.Password});
            return Ok(token);
        }
    }
}