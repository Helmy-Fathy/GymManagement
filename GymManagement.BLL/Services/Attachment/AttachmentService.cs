using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Attachment
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long _maxFileSize = 5 * 1024 * 1024; //5MB
        private readonly ILogger<AttachmentService> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly string[] _allowedExtensions = { ".png", "jpeg", "jpg" };

        public AttachmentService(ILogger<AttachmentService> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }
        public async Task<string?> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream == null || !fileStream.CanRead || fileStream.Length == 0) return null;
            if (fileStream.Length > _maxFileSize)
            {
                _logger.LogError($"File rejected : File Size Is Too Large {fileStream.Length} Bytes");
                return null;
            }

            var extension = Path.GetExtension(fileName);
            if (string.IsNullOrWhiteSpace(extension) || !_allowedExtensions.Contains(extension))
            {
                _logger.LogError($"File rejected : Extension {extension} Not Allowed");
                return null;
            }

            var uploadsFolder = Path.Combine(_env.ContentRootPath, folderName);
            Directory.CreateDirectory(uploadsFolder); // create the folder if it's not found 

            var storedFileName = $"{Guid.NewGuid()}{fileName}";
            var filePath = Path.Combine(uploadsFolder, storedFileName);

            try
            {
                using var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write);
                await fileStream.CopyToAsync(fs, ct);
                return storedFileName;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upload File {fileName}");
                return null;
            }
        }
    }
}
