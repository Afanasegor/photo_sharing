using PhotoSharing.Core.Models.Social;

namespace PhotoSharing.Core.Interfaces.Repositories
{
    public interface IFriendshipRepository
    {
        Task<Friendship> AddFriendship(Friendship friendship);

        Task<bool> AreFriends(Guid userFirstId, Guid userSecondId);
    }
}
