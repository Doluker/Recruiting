
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class DictionaryServiceClient:IDictionaryServiceClient
    {
        private readonly HttpClient _http;
        private ReferenceDataDTO? _cachedData;
        public DictionaryServiceClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<ReferenceDataDTO?> GetBootstrapAsync()
        {
            if (_cachedData != null)
                return _cachedData;
            _cachedData = await _http.GetFromJsonAsync<ReferenceDataDTO>("api/dictionaries/bootstrap");
            return _cachedData;
        }
        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddCityAsync(string name)
        {
            var response = await _http.PostAsJsonAsync("api/dictionaries/cities", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось добавить город");
        }
        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddSkillAsync(string name)
        {
            var response = await _http.PostAsJsonAsync("api/dictionaries/skills", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось добавить навык");
        }
        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddPositionAsync(string name)
        {
            var response = await _http.PostAsJsonAsync("api/dictionaries/positions", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось добавить должность");
        }
        private async Task<(bool Success, string? Error, DictionaryDTO? Item)> ProcessResponse(HttpResponseMessage response, string defaultError)
        {
            if (response.IsSuccessStatusCode)
            {
                var item = await response.Content.ReadFromJsonAsync<DictionaryDTO>();
                _cachedData = null;
                return (true, null, item);
            }
            var err = await response.Content.ReadFromJsonAsync<ApiError>();
            return (false, err?.Message ?? defaultError, null);
        }
        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateSkillAsync(int id, string name)
        {
            var response = await _http.PutAsJsonAsync($"api/dictionaries/skills/{id}", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось обновить навык");
        }

        public async Task<bool> DeleteSkillAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/dictionaries/skills/{id}");
            if (response.IsSuccessStatusCode) { _cachedData = null; return true; }
            return false;
        }

        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateCityAsync(int id, string name)
        {
            var response = await _http.PutAsJsonAsync($"api/dictionaries/cities/{id}", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось обновить город");
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/dictionaries/cities/{id}");
            if (response.IsSuccessStatusCode) { _cachedData = null; return true; }
            return false;
        }

        public async Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdatePositionAsync(int id, string name)
        {
            var response = await _http.PutAsJsonAsync($"api/dictionaries/positions/{id}", new CreateDictionaryItemRequest(name));
            return await ProcessResponse(response, "Не удалось обновить должность");
        }

        public async Task<bool> DeletePositionAsync(int id)
        {
            var response = await _http.DeleteAsync($"api/dictionaries/positions/{id}");
            if (response.IsSuccessStatusCode) { _cachedData = null; return true; }
            return false;
        }
    }
}
