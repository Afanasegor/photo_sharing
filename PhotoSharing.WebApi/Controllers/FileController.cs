using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhotoSharing.Abstractions.Enums;
using PhotoSharing.Abstractions.Interfaces.Common;
using PhotoSharing.Abstractions.Interfaces.Frienship;
using PhotoSharing.Abstractions.Models.Common;
using PhotoSharing.WebApi.Extensions.Utils;

namespace PhotoSharing.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IUserService _userService;
        private readonly IFriendshipService _friendshipService;

        public FileController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        private readonly ILogger<FileController> _logger;

        public FileController(IFileService fileService, IUserService userService, ILogger<FileController> logger, IFriendshipService friendshipService)
        {
            _fileService = fileService;
            _userService = userService;
            _logger = logger;
            _friendshipService = friendshipService;
        }

        [HttpGet("download-file")]
        public async Task<IActionResult> DownloadFile(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return BadRequest("Incorrect id");
            }

            var user = await GetAuthorizedUserFromRequest();

            if (user == null)
            {
                return Forbid("Need to login");
            }

            var fileInfo = await _fileService.GetById(id.Value);

            if (fileInfo == null) 
            {

                return NotFound("File not found");
            }

            if (fileInfo.UserId == null)
            {
                return StatusCode(500);
            }

            if (user.Id != fileInfo.UserId.Value)
            {
                var isAvailableToCheck = await _friendshipService.IsAvailableToCheckContent(user.Id, fileInfo.UserId.Value);
                if (!isAvailableToCheck)
                {
                    return Forbid("User is not your subscriber");
                }
            }

            var fileResult = await _fileService.DownloadFile(fileInfo.FileName);

            return File(fileResult.Item1, fileResult.Item2, fileInfo.FileName);
        }


        [HttpPost("add-file")]
        [RequestSizeLimit(10000000)]
        public async Task<IActionResult> AddFile(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    return BadRequest("No file to upload");
                }

                var user = await GetAuthorizedUserFromRequest();

                if (user == null)
                {
                    return Forbid("Need to login");
                }

                byte[] fileInByteArray;

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    fileInByteArray = ms.ToArray();
                }

                var fileName = file.FileName;
                var mimeType = GetMimeTypeFromFileName(fileName);

                if (mimeType == FileMimeType.Unknown)
                {
                    return BadRequest("Incorrect file mime type");
                }

                var result = await _fileService.CreateFile(fileInByteArray, GenerateUniqueName(fileName), mimeType, user.Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                return StatusCode(500);
            }
            
        }




        #region Utils
        private string GenerateUniqueName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return name;

            var result = DateTime.Now.ToFullString() + "_" + name;
            return result;
        }

        [Obsolete("remove")]
        private string GetFileNameWithoutMime(string fileName)
        {
            var splittedFileName = fileName.Split('.');
            if (splittedFileName.Length > 1)
            {
                return fileName.Replace('.' + splittedFileName.Last(), string.Empty);
            }

            return fileName;
        }

        private FileMimeType GetMimeTypeFromFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return FileMimeType.Unknown;
            }

            if (fileName.EndsWith(".jpg") || fileName.EndsWith(".jpeg"))
                return FileMimeType.Jpeg;


            if (fileName.EndsWith(".png"))
                return FileMimeType.Png;

            return FileMimeType.Unknown;
        }

        // TODO: move to middleware
        private async Task<UserOutputModel> GetAuthorizedUserFromRequest()
        {
            var email = Request.Cookies["username"];
            var user = await _userService.GetUserByEmail(email);
            return user;
        }

        #endregion
    }
}
