using Microsoft.AspNetCore.Components.Forms;
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Recruiting.Client.Services
{
    public class ResumeServiceClient: IResumeServiceClient
    {
        private readonly HttpClient _http;
        public ResumeServiceClient(HttpClient http)
        {
            _http = http;
        }
        public async Task<(UploadResumeResponse? Response, string? Error)> UploadResumeAsync(IBrowserFile file)
        {
            try
            {
                using var content = new MultipartFormDataContent();

                // Ограничение размера: 15 МБ
                var fileContent = new StreamContent(file.OpenReadStream(maxAllowedSize: 15 * 1024 * 1024));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                content.Add(fileContent, "file", file.Name);
                var response = await _http.PostAsync("api/resumes/upload", content);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<UploadResumeResponse>();
                    return (result, null);
                }
                var errorText = await response.Content.ReadAsStringAsync();
                return (null, string.IsNullOrWhiteSpace(errorText) ? "Ошибка загрузки файла" : errorText);
            }
            catch (Exception ex)
            {
                return (null, ex.Message);
            }
        }
        public async Task<ResumeFileDTO?> GetByIdAsync(int id)
        {
            try
            {
                return await _http.GetFromJsonAsync<ResumeFileDTO>($"api/resumes/{id}");
            }
            catch
            {
                return null;
            }
        }
    }
}
