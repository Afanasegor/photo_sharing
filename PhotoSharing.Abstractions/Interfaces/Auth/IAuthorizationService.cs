using PhotoSharing.Abstractions.Models.Auth;

namespace PhotoSharing.Abstractions.Interfaces.Auth
{
    public interface IAuthorizationService
    {
        Task<AuthorizationInfo> GetToken(Credentials credentials);
        Task<AuthorizationInfo> RefreshToken(string username, string refreshToken);
    }
}
