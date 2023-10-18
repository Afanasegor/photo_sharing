using Microsoft.EntityFrameworkCore;
using PhotoSharing.Abstractions.Interfaces.Common;
using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.Abstractions.Utils;
using PhotoSharing.Core.Interfaces.Repositories;

namespace PhotoSharing.Services.Services.Common
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserOutputModel>> GetAllUsers()
        {
            var allUsers = await _userRepository.Get().AsNoTracking()
                .Include(x => x.Subscriptions)
                .ThenInclude(x => x.UserSecond)
                .Include(x => x.Subscribers)
                .ThenInclude(x => x.UserFirst)
                .Include(x => x.Files)
                .ToListAsync();

            var result = new List<UserOutputModel>();

            foreach (var user in allUsers)
            {
                result.Add(user.ConvertUserToOutputModel());
            }

            return result;
        }

        public async Task<UserOutputModel> GetUserByEmail(string email)
        {
            var user = await _userRepository.Get().Include(r => r.Role).FirstOrDefaultAsync(x => x.Email == email);
            return user.ConvertUserToOutputModel();
        }
    }
}
