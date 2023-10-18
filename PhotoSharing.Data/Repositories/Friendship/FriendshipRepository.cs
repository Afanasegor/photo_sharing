using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace PhotoSharing.Data.Repositories.Friendship
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly ApplicationContext _context;

        public FriendshipRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Core.Models.Social.Friendship> AddFriendship(Core.Models.Social.Friendship friendship)
        {
            var friendshipInDb = await _context.Friendships.AsNoTracking().FirstOrDefaultAsync(x => x.UserFirstId == friendship.UserFirstId &&
                                                                                 x.UserSecondId == friendship.UserSecondId);

            if (friendshipInDb != null)
            {
                return friendshipInDb;
            }

            await _context.AddAsync(friendship);
            await _context.SaveChangesAsync();
            return friendship;
        }

        public async Task<bool> AreFriends(Guid userFirstId, Guid userSecondId)
        {
            var friendships = await _context.Friendships.AsNoTracking().Where(x => (x.UserFirstId == userFirstId && x.UserSecondId == userSecondId) ||
                                                    (x.UserSecondId == userFirstId && x.UserSecondId == userFirstId)).ToListAsync();

            if (friendships.Count == 2)
            {
                return true;
            }

            return false;
        }
    }
}
