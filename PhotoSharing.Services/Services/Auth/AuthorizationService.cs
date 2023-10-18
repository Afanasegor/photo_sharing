using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PhotoSharing.Abstractions.Interfaces.Auth;
using PhotoSharing.Abstractions.Models.Auth;
using PhotoSharing.Core.Interfaces.Repositories;
using PhotoSharing.Core.Models.Auth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PhotoSharing.Services.Services.Auth
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserRepository _usersRepository;

        public AuthorizationService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<AuthorizationInfo> GetToken(Credentials credentials)
        {
            var user = await GetUserByUsername(credentials.Login);

            if (user is null)
            {
                throw new ArgumentException("User was not found");
            }

            var isPasswordCorrect = await _usersRepository.CheckPassword(user, credentials.Password);

            if (!isPasswordCorrect)
            {
                throw new ArgumentException("Incorrect password");
            }

            var identity = GetIdentity(user);

            var encodedJwt = WriteJwtToken(identity);

            // update user refresh token
            var refreshToken = GenerateRefreshToken();
            refreshToken.UserName = identity.Name;

            user.RefreshToken = refreshToken.Token;
            user.RefreshTokenCreatingDate = refreshToken.Created;
            user.RefreshTokenExpiringDate = refreshToken.Expires;

            await _usersRepository.Update(user);

            var result = new AuthorizationInfo
            {
                Token = encodedJwt,
                UserName = identity.Name,
                RefreshToken = refreshToken
            };

            return result;
        }

        public async Task<AuthorizationInfo> RefreshToken(string username, string refreshToken)
        {
            var userByUsername = await GetUserByUsername(username);

            if (userByUsername == null)
            {
                throw new ArgumentException($"Unknown user with name: [{username}]");
            }

            if (string.IsNullOrWhiteSpace(userByUsername.RefreshToken) || userByUsername.RefreshToken != refreshToken)
            {
                throw new ArgumentException($"Incorrect refresh token. RefreshToken: [{refreshToken}]; Expected: [{userByUsername.RefreshToken}]");
            }

            if (userByUsername.RefreshTokenExpiringDate == null || userByUsername.RefreshTokenExpiringDate < DateTime.Now)
            {
                throw new ArgumentException("User token expired");
            }

            var identity = GetIdentity(userByUsername);

            var encodedJwt = WriteJwtToken(identity);

            var result = new AuthorizationInfo
            {
                Token = encodedJwt,
                UserName = identity.Name
            };

            return result;
        }

        private async Task<User> GetUserByUsername(string username)
        {
            var user = await _usersRepository.Get().Include(r => r.Role).FirstOrDefaultAsync(x => x.Email == username);
            return user;
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("Error to get identity of user");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }


        /// <returns>New encodedJwt jwt token</returns>
        private string WriteJwtToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
            notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.ACCESS_TOKEN_LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var now = DateTime.UtcNow;
            var expires = now.Add(TimeSpan.FromHours(AuthOptions.REFRESH_TOKEN_LIFETIME));

            var result = new RefreshToken()
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Created = now,
                Expires = expires
            };

            return result;
        }
    }

    // TODO: move to app settings | env
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int ACCESS_TOKEN_LIFETIME = 10; // время жизни токена - 1 минута
        public const int REFRESH_TOKEN_LIFETIME = 24; // время жизни рефреш токена в часах
        public static SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}
