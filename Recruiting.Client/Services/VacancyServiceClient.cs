using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class VacancyServiceClient:IVacancyServiceClient
    {
        private readonly HttpClient _http;
        public VacancyServiceClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<PagedResult<VacancyDTO>?> GetVacanciesAsync(VacancyFilter? filter = null)
        {
            filter ??= new VacancyFilter();
            var query = $"api/vacancies?search={Uri.EscapeDataString(filter.Search ?? "")}&page={filter.Page}&pageSize={filter.PageSize}";
            if (filter.OnlyActive.HasValue) query += $"&onlyActive={filter.OnlyActive.Value}";
            if (filter.StatusId.HasValue) query += $"&statusId={filter.StatusId.Value}";
            return await _http.GetFromJsonAsync<PagedResult<VacancyDTO>>(query);
        }
        public async Task<VacancyDTO?> GetVacancyByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<VacancyDTO>($"api/vacancies/{id}");
        }
        public async Task<(bool Success, string? Error, int? Id)> CreateVacancyAsync(CreateVacancyRequest request)
        {
            var response = await _http.PostAsJsonAsync("api/vacancies", request);
            if (response.IsSuccessStatusCode)
            {
                var res = await response.Content.ReadFromJsonAsync<OperationResult>();
                return (true, null, res?.EntityId);
            }
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            return (false, error?.Message ?? "Не удалось создать вакансию", null);
        }
        public async Task<(bool Success, string? Error)> UpdateVacancyAsync(int id, UpdateVacancyRequest request)
        {
            var response = await _http.PutAsJsonAsync($"api/vacancies/{id}", request);
            if (response.IsSuccessStatusCode) return (true, null);
            var error = await response.Content.ReadFromJsonAsync<ApiError>();
            return (false, error?.Message ?? "Не удалось обновить вакансию");
        }
        public async Task<bool> DeleteVacancyAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/vacancies/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}
