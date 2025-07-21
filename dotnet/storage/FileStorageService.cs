using Microsoft.AspNetCore.Identity;

namespace Storage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string basePath = Path.Combine(Directory.GetCurrentDirectory(), "files");

        public async Task<string> SaveFileAsync(Stream file, string fileName, string folder)
        {
            var normalizedFolder = folder.ToLower();
            var folderPath = Path.Combine(basePath, normalizedFolder);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            using var outputFile = new FileStream(Path.Combine(folderPath, fileName), FileMode.Create);
            await file.CopyToAsync(outputFile);

            return Path.Combine(normalizedFolder, fileName).Replace("\\", "/");
        }

        public async Task<Stream> GetFileAsync(string path)
        {
            var normalizedPath = path.Replace("/", "\\");
            var filePath = Path.Combine(basePath, normalizedPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo no encontrado", filePath);

            var memory = new MemoryStream();
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            await stream.CopyToAsync(memory);
            memory.Position = 0;
            return memory;
        }


        public Task DeleteFileAsync(string path)
        {
            var normalizedPath = path.Replace("/", "\\");
            var filePath = Path.Combine(basePath, normalizedPath);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            return Task.CompletedTask;
        }

    }
}