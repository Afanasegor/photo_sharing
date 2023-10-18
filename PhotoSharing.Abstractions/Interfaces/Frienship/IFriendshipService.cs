namespace PhotoSharing.Abstractions.Interfaces.Frienship
{
    public interface IFriendshipService
    {
        Task AddFriend(Guid userFirstId, Guid userSecondId);
        Task<bool> AreFriends(Guid userFirstId, Guid userSecondId);

        /// <param name="userFirstId">original user</param>
        /// <param name="userSecondId">user to check is sub or not</param>
        Task<bool> IsAvailableToCheckContent(Guid userFirstId, Guid userSecondId);
    }
}
