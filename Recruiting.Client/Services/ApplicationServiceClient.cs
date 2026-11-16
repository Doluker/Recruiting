using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class ApplicationServiceClient: IApplicationServiceClient
    {
        private readonly HttpClient _http;
        public ApplicationServiceClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<PagedResult<ApplicationDTO>?> GetApplicationsAsync(ApplicationFilter filter)
        {
            var query = $"api/applications?page={filter.Page}&pageSize={filter.PageSize}";
            if (filter.VacancyId.HasValue) query += $"&vacancyId={filter.VacancyId.Value}";
            if (filter.CandidateId.HasValue) query += $"&candidateId={filter.CandidateId.Value}";
            if (filter.StatusId.HasValue) query += $"&statusId={filter.StatusId.Value}";
            return await _http.GetFromJsonAsync<PagedResult<ApplicationDTO>>(query);
        }
        public async Task<ApplicationDTO?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<ApplicationDTO>($"api/applications/{id}");
        }
        public async Task<List<StatusHistoryDTO>?> GetHistoryAsync(int id)
        {
            return await _http.GetFromJsonAsync<List<StatusHistoryDTO>>($"api/applications/{id}/history");
        }
        public async Task<(bool Success, string? Error)> ChangeStageAsync(int applicationId, ChangeStageRequest request)
        {
            var response = await _http.PutAsJsonAsync($"api/applications/{applicationId}/stage", request);
            if (response.IsSuccessStatusCode) return (true, null);
            var err = await response.Content.ReadFromJsonAsync<ApiError>();
            return (false, err?.Message ?? "Ошибка изменения этапа");
        }
    }
}
