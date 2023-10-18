using PhotoSharing.Core.Models.Auth;

namespace PhotoSharing.Core.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<bool> CheckPassword(User user, string pwd);
        Task SetPassword(User user, string pwd);
    }
}
