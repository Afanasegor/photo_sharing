using PhotoSharing.Abstractions.Enums;
using PhotoSharing.Core.Enums.Common;

namespace PhotoSharing.Abstractions.Utils
{
    public static class FileMimeConverter
    {
        public static MimeType ConvertToCore(this FileMimeType mimeType)
        {
            switch(mimeType) 
            {
                case FileMimeType.Jpeg: 
                    return MimeType.Jpeg;
                case FileMimeType.Png: 
                    return MimeType.Png;
                default: 
                    return MimeType.Unknown;
            }
        }

        public static string ConvertToString(this FileMimeType mimeType)
        {
            switch (mimeType)
            {
                case FileMimeType.Jpeg:
                    return ".jpeg";
                case FileMimeType.Png:
                    return ".png";
                default:
                    return string.Empty;
            }
        }

        public static FileMimeType ConvertToServiceOutputModel(this MimeType mimeType)
        {
            switch (mimeType)
            {
                case MimeType.Jpeg:
                    return FileMimeType.Jpeg;
                case MimeType.Png:
                    return FileMimeType.Png;
                default:
                    return FileMimeType.Unknown;
            }
        }
    }
}
