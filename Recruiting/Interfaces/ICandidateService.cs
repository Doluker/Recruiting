using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface ICandidateService
    {
        Task<PagedResult<CandidateDTO>> GetCandidatesAsync(CandidateFilter filter);
        Task<CandidateDTO?> GetCandidateByIdAsync(int id);
        Task<(bool Success, string? Error, int? Id)> CreateCandidateAsync(CreateCandidateRequest request);
        Task<(bool Success, string? Error)> UpdateCandidateAsync(int id, UpdateCandidateRequest request);
        Task<(bool Success, string? Error)> DeleteCandidateAsync(int id);
    }
}
