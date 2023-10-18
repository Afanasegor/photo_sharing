namespace PhotoSharing.Abstractions.Models.Common
{
    public class UserOutputModel
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }

        public List<UserOutputModel> Subscribers { get; set; } = new List<UserOutputModel>();

        public List<UserOutputModel> Subscriptions { get; set; } = new List<UserOutputModel>();

        public List<FileInfoOutputModel> Files { get; set; } = new List<FileInfoOutputModel>();
    }
}
