using PhotoSharing.Abstractions.Enums;
using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.Core.Enums.Common;

namespace PhotoSharing.Abstractions.Interfaces.Common
{
    public interface IFileService
    {
        Task<FileInfoOutputModel> GetById(Guid id);
        Task<(byte[], string)> DownloadFile(string fileName);
        Task<FileInfoOutputModel> CreateFile(byte[] blob, string fileName, FileMimeType mime, Guid userId);
    }
}
