using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhotoSharing.Abstractions.Interfaces.Common;

namespace PhotoSharing.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllUsers()
        {

            var request = Request;
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }
    }
}
