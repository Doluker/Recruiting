using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IVacancyService
    {
        Task<PagedResult<VacancyDTO>> GetVacanciesAsync(VacancyFilter filter);
        Task<VacancyDTO?> GetVacancyByIdAsync(int id);
        Task<(bool Success, string? Error, int? Id)> CreateVacancyAsync(CreateVacancyRequest request);
        Task<(bool Success, string? Error)> UpdateVacancyAsync(int id, UpdateVacancyRequest request);
        Task<(bool Success, string? Error)> ChangeStatusAsync(int id, int statusId);
        Task<(bool Success, string? Error)> DeleteAsync(int id);
    }
}
