using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class CandidateServiceClient: ICandidateServiceClient
    {
        private readonly HttpClient _http;
        public CandidateServiceClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<PagedResult<CandidateDTO>?> GetCandidatesAsync(CandidateFilter? filter = null)
        {
            filter ??= new CandidateFilter();
            var query = $"api/candidates?page={filter.Page}&pageSize={filter.PageSize}";
            if (!string.IsNullOrWhiteSpace(filter.Search)) query += $"&search={Uri.EscapeDataString(filter.Search)}";
            return await _http.GetFromJsonAsync<PagedResult<CandidateDTO>>(query);
        }
        public async Task<CandidateDTO?> GetByIdAsync(int id)
        {
            return await _http.GetFromJsonAsync<CandidateDTO>($"api/candidates/{id}");
        }
    }
}
