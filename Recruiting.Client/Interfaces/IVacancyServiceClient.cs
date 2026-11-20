using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IVacancyServiceClient
    {
        Task<PagedResult<VacancyDTO>?> GetVacanciesAsync(VacancyFilter? filter = null);
        Task<VacancyDTO?> GetVacancyByIdAsync(int id);
        Task<(bool Success, string? Error, int? Id)> CreateVacancyAsync(CreateVacancyRequest request);
        Task<(bool Success, string? Error)> UpdateVacancyAsync(int id, UpdateVacancyRequest request);
        Task<bool> DeleteVacancyAsync(int id);
    }
}
