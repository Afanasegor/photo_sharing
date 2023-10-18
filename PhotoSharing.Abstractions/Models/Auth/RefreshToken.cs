namespace PhotoSharing.Abstractions.Models.Auth
{
    public class RefreshToken
    {
        public string Token { get; set; }

        public string UserName { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Expires { get; set; }
    }
}
