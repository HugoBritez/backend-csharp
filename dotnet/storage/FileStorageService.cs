using System.Security.Cryptography;

namespace Storage
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string basePath = Path.Combine(Directory.GetCurrentDirectory(), "files");
        private readonly long maxFileSize = 100 * 1024 * 1024; // 100MB
        private readonly string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".pdf", ".doc", ".docx" };

        public async Task<string> SaveFileAsync(Stream file, string fileName, string folder)
        {
            // Validaciones
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo no puede estar vacío");

            if (file.Length > maxFileSize)
                throw new ArgumentException($"El archivo excede el tamaño máximo de {maxFileSize / 1024 / 1024}MB");

            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(extension))
                throw new ArgumentException($"Tipo de archivo no permitido: {extension}");

            // Sanitizar nombre del archivo
            var sanitizedFileName = SanitizeFileName(fileName);
            var normalizedFolder = folder.ToLowerInvariant();
            var folderPath = Path.Combine(basePath, normalizedFolder);
            
            Directory.CreateDirectory(folderPath);

            // Generar nombre único para evitar colisiones
            var uniqueFileName = GenerateUniqueFileName(folderPath, sanitizedFileName);
            var filePath = Path.Combine(folderPath, uniqueFileName);

            using var outputFile = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(outputFile);

            return Path.Combine(normalizedFolder, uniqueFileName).Replace("\\", "/");
        }

        public async Task<Stream> GetFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("La ruta del archivo no puede estar vacía");

            // Normalizar el path para que sea consistente
            var pathParts = path.Split('/', '\\');
            var normalizedPathParts = pathParts.Select((part, index) => 
                index == 0 ? part : part.ToLowerInvariant()).ToArray();
            
            var normalizedPath = string.Join(Path.DirectorySeparatorChar, normalizedPathParts);
            var filePath = Path.Combine(basePath, normalizedPath);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("Archivo no encontrado", filePath);

            // Para archivos pequeños, usar MemoryStream
            var fileInfo = new FileInfo(filePath);
            if (fileInfo.Length < 1024 * 1024) // 1MB
            {
                var memory = new MemoryStream();
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                await stream.CopyToAsync(memory);
                memory.Position = 0;
                return memory;
            }
            
            // Para archivos grandes, devolver FileStream directamente
            return new FileStream(filePath, FileMode.Open, FileAccess.Read);
        }

        public Task DeleteFileAsync(string path)
        {
            if (string.IsNullOrEmpty(path))
                return Task.CompletedTask;

            var normalizedPath = path.Replace("/", "\\");
            var filePath = Path.Combine(basePath, normalizedPath);
            
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            
            return Task.CompletedTask;
        }

        private string SanitizeFileName(string fileName)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = fileName;
            
            foreach (var invalidChar in invalidChars)
            {
                sanitized = sanitized.Replace(invalidChar, '_');
            }
            
            return sanitized;
        }

        private static  string GenerateUniqueFileName(string folderPath, string fileName)
        {
            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var counter = 0;
            var uniqueName = fileName;

            while (File.Exists(Path.Combine(folderPath, uniqueName)))
            {
                counter++;
                uniqueName = $"{nameWithoutExt}_{counter}{extension}";
            }

            return uniqueName;
        }
        
    }
}