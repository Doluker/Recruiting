using Microsoft.AspNetCore.Components.Forms;
using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IResumeServiceClient
    {
        Task<(UploadResumeResponse? Response, string? Error)> UploadResumeAsync(IBrowserFile file);
        Task<ResumeFileDTO?> GetByIdAsync(int id);
    }
}
