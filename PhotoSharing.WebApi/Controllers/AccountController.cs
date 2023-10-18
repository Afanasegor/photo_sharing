using Microsoft.AspNetCore.Mvc;
using PhotoSharing.Abstractions.Interfaces.Auth;
using PhotoSharing.Abstractions.Models.Auth;

namespace PhotoSharing.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accounts;

        public AccountController(IAccountService accounts)
        {
            _accounts = accounts;
        }

        [HttpPost("register")]
        public async Task<IActionResult> SignUp(RegistrationData registrationData)
        {
            await _accounts.SignUp(registrationData);
            return Ok();
        }
    }
}
