using Newtonsoft.Json;

namespace PhotoSharing.Abstractions.Models.Auth
{
    public class AuthorizationInfo
    {
        public string Token { get; set; }

        public string UserName { get; set; }

        [JsonIgnore]
        public RefreshToken RefreshToken { get; set; }
    }
}
