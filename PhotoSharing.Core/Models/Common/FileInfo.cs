using PhotoSharing.Core.Enums.Common;
using PhotoSharing.Core.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSharing.Core.Models.Common
{
    [Table("file_infos", Schema = "common")]
    public class FileInfo : BaseEntity
    {
        [Column("file_name")]
        public string FileName { get; set; }

        [Column("mime_type")]
        public MimeType MimeType { get; set; } = MimeType.Unknown;

        [Column("user_id")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
