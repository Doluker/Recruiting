using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IApplicationServiceClient
    {
        Task<PagedResult<ApplicationDTO>?> GetApplicationsAsync(ApplicationFilter filter);
        Task<ApplicationDTO?> GetByIdAsync(int id);
        Task<List<StatusHistoryDTO>?> GetHistoryAsync(int id);
        Task<(bool Success, string? Error)> ChangeStageAsync(int applicationId, ChangeStageRequest request);
    }
}
