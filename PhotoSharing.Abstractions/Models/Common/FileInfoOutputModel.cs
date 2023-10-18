using PhotoSharing.Abstractions.Enums;

namespace PhotoSharing.Abstractions.Models.Common
{
    public class FileInfoOutputModel
    {
        public Guid Id { get; set; }
        public string FullPath { get; set; }

        public string FileName { get; set; }

        public FileMimeType MimeType { get; set; }

        public Guid? UserId { get; set; }
    }
}
