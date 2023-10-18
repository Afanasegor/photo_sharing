using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoSharing.Abstractions.Models.Auth;
using IAuthorizationService = PhotoSharing.Abstractions.Interfaces.Auth.IAuthorizationService;

namespace PhotoSharing.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<AuthorizationController> _logger;

        public AuthorizationController(IAuthorizationService authorizationService, ILogger<AuthorizationController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
        }

        [Authorize]
        [HttpGet("check-authorized")]
        public async Task<IActionResult> CheckAuthorized()
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Credentials loginPass)
        {
            var result = await _authorizationService.GetToken(loginPass);

            SetRefreshTokenCookies(result.RefreshToken);

            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var username = Request.Cookies["username"];

            if (string.IsNullOrWhiteSpace(refreshToken) || string.IsNullOrWhiteSpace(username))
            {
                return Unauthorized();
            }

            var token = await _authorizationService.RefreshToken(username, refreshToken);
            return Ok(token);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("username");

            return Ok();
        }


        private void SetRefreshTokenCookies(RefreshToken refreshToken)
        {
            if (refreshToken == null)
            {
                _logger.LogWarning("Cookies were not set");
                return;
            }

            var cookieOptions = new CookieOptions()
            {
                HttpOnly = true,
                Expires = refreshToken.Expires,
            };

            Response.Cookies.Delete("refreshToken");
            Response.Cookies.Delete("username");

            Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);
            Response.Cookies.Append("username", refreshToken.UserName, cookieOptions);
        }
    }
}
