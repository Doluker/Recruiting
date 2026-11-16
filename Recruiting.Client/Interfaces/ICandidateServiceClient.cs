using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface ICandidateServiceClient
    {
        Task<PagedResult<CandidateDTO>?> GetCandidatesAsync(CandidateFilter? filter = null);
        Task<CandidateDTO?> GetByIdAsync(int id);
    }
}
