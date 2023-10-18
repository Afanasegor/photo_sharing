using Microsoft.EntityFrameworkCore;
using PhotoSharing.Abstractions.Interfaces.Frienship;
using PhotoSharing.Core.Interfaces.Repositories;
using System.Diagnostics;

namespace PhotoSharing.Services.Services.Friendship
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;

        public FriendshipService(IFriendshipRepository friendshipRepository, IUserRepository userRepository)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
        }

        public async Task AddFriend(Guid userFirstId, Guid userSecondId)
        {
            await _friendshipRepository.AddFriendship(new Core.Models.Social.Friendship() { UserFirstId = userFirstId, UserSecondId = userSecondId});
        }

        [Obsolete("incorrect logic,, not for the task")]
        public async Task<bool> AreFriends(Guid userFirstId, Guid userSecondId)
        {
            return await _friendshipRepository.AreFriends(userFirstId, userSecondId);
        }

        /// <param name="userFirstId">original user</param>
        /// <param name="userSecondId">user to check is sub or not</param>
        public async Task<bool> IsAvailableToCheckContent(Guid userFirstId, Guid userSecondId)
        {
            var user =  await _userRepository.Get().AsNoTracking().Include(x => x.Subscribers).FirstOrDefaultAsync(x => x.Id == userFirstId);

            if (user == null)
            {
                throw new ArgumentException("Original User was not found");
            }

            var subscriber = user.Subscribers.FirstOrDefault(x => x.UserFirstId == userSecondId);

            if (subscriber != null)
            {
                return true;
            }

            return false;
        }
    }
}
