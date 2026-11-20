namespace Recruiting.Interfaces
{
    public interface IStorageService
    {
        Task EnsureBucketExistsAsync(CancellationToken ct = default);
        Task<string> UploadAsync(Stream stream, string fileName, string contentType, long size, CancellationToken ct = default);
        Task<Stream> DownloadAsync(string objectKey, CancellationToken ct = default);
        Task DeleteAsync(string objectKey, CancellationToken ct = default);
    }
}
