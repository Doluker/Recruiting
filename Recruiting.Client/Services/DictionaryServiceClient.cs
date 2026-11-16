
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
    }
}
