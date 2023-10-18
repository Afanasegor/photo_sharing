using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoSharing.Abstractions.Interfaces.Common;
using PhotoSharing.Abstractions.Interfaces.Frienship;
using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.Abstractions.Models.Friendship;

namespace PhotoSharing.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;
        private readonly IUserService _userService;

        public FriendshipController(IFriendshipService friendshipService, IUserService userService)
        {
            _friendshipService = friendshipService;
            _userService = userService;
        }



        [HttpGet("is-subscriber")]
        public async Task<IActionResult> IsSubscriber(Guid? subscriberId)
        {
            if (subscriberId == null || subscriberId == Guid.Empty)
            {
                return BadRequest("fill correct data");
            }

            var user = await GetAuthorizedUserFromRequest();

            if (user == null)
            {
                return Forbid("Need to login");
            }

            if (user.Id == subscriberId.Value)
            {
                return BadRequest("It's your id");
            }

            var result = await _friendshipService.IsAvailableToCheckContent(user.Id, subscriberId.Value);
            return Ok(result);
        }

        [HttpPost("add-to-friend")]
        public async Task<IActionResult> AddToFriends(AddToFriendsInputModel input)
        {
            if (input == null || input.UserIdToAdd == Guid.Empty)
            {
                return BadRequest("fill correct data");
            }

            var user = await GetAuthorizedUserFromRequest();

            if (user == null)
            {
                return Forbid("Need to login");
            }

            if (user.Id == input.UserIdToAdd)
            {
                return BadRequest("Can't add yourself to friends");
            }

            await _friendshipService.AddFriend(user.Id, input.UserIdToAdd);
            return Ok("Request was sent");
        }

        private async Task<UserOutputModel> GetAuthorizedUserFromRequest()
        {
            var email = Request.Cookies["username"];
            var user = await _userService.GetUserByEmail(email);
            return user;
        }
    }
}
