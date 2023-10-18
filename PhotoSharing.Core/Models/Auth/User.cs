using PhotoSharing.Core.Models.Common;
using System.ComponentModel.DataAnnotations.Schema;
using FileInfo = PhotoSharing.Core.Models.Common.FileInfo;

namespace PhotoSharing.Core.Models.Auth
{
    [Table("users", Schema = "auth")]
    public class User : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password_hash")]
        public string PasswordHash { get; set; }

        [Column("refresh_token")]
        public string RefreshToken { get; set; }

        [Column("token_created")]
        public DateTime? RefreshTokenCreatingDate { get; set; }

        [Column("token_expires")]
        public DateTime? RefreshTokenExpiringDate { get; set; }




        [Column("role_id")]
        public Guid? RoleId { get; set; }
        public Role Role { get; set; }


        public virtual IList<Social.Friendship> Subscriptions { get; set; }
        public virtual IList<Social.Friendship> Subscribers { get; set; }


        private readonly IList<FileInfo> _files = new List<FileInfo>();
        public IReadOnlyCollection<FileInfo> Files => _files.AsReadOnly();
    }
}
