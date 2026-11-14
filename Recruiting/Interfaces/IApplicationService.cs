using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IApplicationService
    {
        Task<PagedResult<ApplicationDTO>> GetApplicationsAsync(ApplicationFilter filter);
        Task<ApplicationDTO?> GetApplicationByIdAsync(int id);
        Task<List<StatusHistoryDTO>> GetHistoryAsync(int applicationId);
        Task<(bool Success, string? Error, int? Id)> CreateApplicationAsync(CreateApplicationRequest request);
        Task<(bool Success, string? Error)> ChangeStageAsync(int id, ChangeStageRequest request);
        Task<(bool Success, string? Error)> DeleteApplicationAsync(int id);
    }
}
