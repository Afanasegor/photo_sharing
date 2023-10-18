using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Logging;
using PhotoSharing.Abstractions.Enums;
using PhotoSharing.Abstractions.Interfaces.Common;
using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.Abstractions.Utils;
using PhotoSharing.Configuration;
using PhotoSharing.Core.Interfaces.Repositories;
using System.Text.RegularExpressions;
using FileInfo = PhotoSharing.Core.Models.Common.FileInfo;

namespace PhotoSharing.Services.Services.Common
{
    public class FileService : IFileService
    {
        private readonly IBaseRepository<FileInfo> _fileInfoRepository;

        private readonly string _basePath;
        private readonly ILogger<FileService> _logger;

        public FileService(AppConfig appConfig, ILogger<FileService> logger, IBaseRepository<FileInfo> fileInfoRepository)
        {
            _basePath = appConfig.FileDirectory;
            _logger = logger;
            _fileInfoRepository = fileInfoRepository;
        }

        public async Task<FileInfoOutputModel> GetById(Guid id)
        {
            var outputResult = new FileInfoOutputModel();

            var result = await _fileInfoRepository.GetById(id);
            if (result == null)
                return outputResult;

            outputResult = result.ConvertToServiceOutputModel();

            return outputResult;
        }

        public async Task<(byte[], string)> DownloadFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("No file name");

            var filePath = Path.Combine(_basePath, fileName);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await File.ReadAllBytesAsync(filePath);
            return (bytes, contentType);
        }

        public async Task<FileInfoOutputModel> CreateFile(byte[] blob, string fileName, FileMimeType fileMimeType, Guid userId)
        {
            var result = new FileInfoOutputModel();

            try
            {
                var filePath = await SaveFileAsync(blob, _basePath, fileName, fileMimeType.ConvertToString());
                if (blob == null || blob.Length == 0)
                    throw new ArgumentException("Input file is empty");

                var fileLength = blob.Length;
                var fileInfo = new FileInfo()
                {
                    Id = Guid.NewGuid(),
                    FileName = fileName,
                    MimeType = fileMimeType.ConvertToCore(),
                    UserId = userId
                };

                var createdFile = await _fileInfoRepository.Create(fileInfo);

                result.Id = createdFile.Id;
                result.FileName = createdFile.FileName;
                result.MimeType = createdFile.MimeType.ConvertToServiceOutputModel();

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Save file on disk
        /// </summary>
        /// <param name="path">Where to save</param>
        /// <param name="mime">FileFormat in string format</param>
        /// <returns>File full path</returns>
        private async Task<string> SaveFileAsync(byte[] blob, string path, string fileName, string mime)
        {
            try
            {
                var result = string.Empty;

                if (blob == null || blob.Length == 0)
                    throw new ArgumentException("There is no blob");

                if (string.IsNullOrWhiteSpace(fileName) || string.IsNullOrWhiteSpace(mime))
                    throw new ArgumentException("Problem with one of parameters (fileName | mime)");

                var fileNameWithMime = fileName;

                // Check for dots in fileNameWithMime (using regex)
                var normalizedFileName = Regex.Replace(fileNameWithMime, @"/.+", ".");

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var fullPath = Path.Combine(path, normalizedFileName);

                await File.WriteAllBytesAsync(fullPath, blob);

                result = fullPath;
                return result;
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                throw;
            }
        }
    }
}
