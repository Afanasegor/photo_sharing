using PhotoSharing.Abstractions.Models.Common;

namespace PhotoSharing.Abstractions.Utils
{
    public static class FileInfoConverter
    {
        /// <returns>Returns null if fileInfo null</returns>
        public static FileInfoOutputModel ConvertToServiceOutputModel(this Core.Models.Common.FileInfo fileInfo)
        {
            var result = new FileInfoOutputModel();

            if (fileInfo == null)
            {
                return null;
            }

            result.FileName = fileInfo.FileName;
            result.MimeType = fileInfo.MimeType.ConvertToServiceOutputModel();
            result.Id = fileInfo.Id;
            result.UserId = fileInfo.UserId;

            return result;
        }
    }
}
