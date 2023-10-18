using Microsoft.AspNetCore.Identity;
using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Core.Models.Auth;
using PhotoSharing.Data.Context;

namespace PhotoSharing.Data.Repositories.Auth
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly IPasswordHasher<User> _hasher;

        public UserRepository(ApplicationContext applicationContext, IPasswordHasher<User> hasher)
            : base(applicationContext)
        {
            _hasher = hasher;
        }

        public Task<bool> CheckPassword(User user, string pwd)
        {
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash ?? string.Empty, pwd);
            return Task.FromResult(result == PasswordVerificationResult.Success);
        }

        public async Task SetPassword(User user, string pwd)
        {
            user.PasswordHash = _hasher.HashPassword(user, pwd);
            await Update(user);
        }
    }
}
