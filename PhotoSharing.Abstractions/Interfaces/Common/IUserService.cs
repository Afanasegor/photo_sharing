using PhotoSharing.Abstractions.Models.Common;

namespace PhotoSharing.Abstractions.Interfaces.Common
{
    public interface IUserService
    {
        Task<UserOutputModel> GetUserByEmail(string email);
        Task<List<UserOutputModel>> GetAllUsers();
    }
}
