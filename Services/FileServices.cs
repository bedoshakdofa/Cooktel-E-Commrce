using Cooktel_E_commrece.Interfaces;

namespace Cooktel_E_commrece.Services
{
    public class FileServices : IFileService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<FileServices> _logger;
        public FileServices(IWebHostEnvironment env, ILogger<FileServices> logger)
        {
            _env = env;
            _logger = logger;
        }
        public async Task<string> UploadFile(IFormFile file , string[] allowExtensions)
        {

            if (file == null)
            {

                throw new ArgumentNullException(nameof(file));
            }
                

            var ext = Path.GetExtension(file.FileName);

            if (!allowExtensions.Contains(ext))
            {
                throw new ArgumentException($"only allowed extenstions {string.Join(',', allowExtensions)}");
            }

            var path = Path.Combine(_env.WebRootPath, "Uploads");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                _logger.LogInformation(path);
            }


            var fileName = $"{Guid.NewGuid()}-{ext}";
           var fileNameWithPath = Path.Combine(path, fileName);
           using var stream = new FileStream(fileNameWithPath, FileMode.Create);

            await file.CopyToAsync(stream);            
            return fileName;
        }

        public void DeleteFile(string fileName) { 

            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException(nameof(fileName));

            var path = Path.Combine(_env.WebRootPath, "Uploads", fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Invaild file Path");
            }

            File.Delete(path);
        }
    }
}
