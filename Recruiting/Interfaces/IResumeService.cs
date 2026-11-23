using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IResumeService
    {
        Task<UploadResumeResponse> UploadResumeAsync(IFormFile file, CancellationToken ct = default);
        Task<ResumeFileDTO?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<(Stream stream, string contentType, string fileName)> DownloadResumeFileAsync(int id, CancellationToken ct = default);
    }
}
