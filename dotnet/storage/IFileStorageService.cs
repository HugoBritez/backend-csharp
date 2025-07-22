namespace Storage
{
    public interface IFileStorageService

    {
        Task<string> SaveFileAsync(Stream file, string fileName, string folder);
        Task<Stream> GetFileAsync(string path);
        Task DeleteFileAsync(string path);
    }
}