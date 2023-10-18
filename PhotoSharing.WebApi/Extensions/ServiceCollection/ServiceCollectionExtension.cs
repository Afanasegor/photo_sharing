using Microsoft.AspNetCore.Identity;
using PhotoSharing.Abstractions.Interfaces.Auth;
using PhotoSharing.Abstractions.Interfaces.Common;
using PhotoSharing.Abstractions.Interfaces.Frienship;
using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Core.Models.Auth;
using PhotoSharing.Data.Repositories;
using PhotoSharing.Data.Repositories.Auth;
using PhotoSharing.Data.Repositories.Friendship;
using PhotoSharing.Services.Services.Auth;
using PhotoSharing.Services.Services.Common;
using PhotoSharing.Services.Services.Friendship;

namespace PhotoSharing.WebApi.Extensions.ServiceCollection
{
    public static class ServiceCollectionExtension
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
            services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IFriendshipRepository, FriendshipRepository>();
        }
        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();

            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        }
    }
}
