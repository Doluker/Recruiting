using Recruiting.Shared.DTOs;

namespace Recruiting.Client.Interfaces
{
    public interface IDictionaryServiceClient
    {
        Task<ReferenceDataDTO?> GetBootstrapAsync();
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddCityAsync(string name);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddSkillAsync(string name);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddPositionAsync(string name);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateCityAsync(int id, string name);
        Task<bool> DeleteCityAsync(int id);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateSkillAsync(int id, string name);
        Task<bool> DeleteSkillAsync(int id);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdatePositionAsync(int id, string name);
        Task<bool> DeletePositionAsync(int id);
    }
}
