using Recruiting.Shared.DTOs;

namespace Recruiting.Interfaces
{
    public interface IDictionaryService
    {
        Task<ReferenceDataDTO> GetBootstrapDataAsync();
        Task<List<DictionaryDTO>> GetCitiesAsync(string? search = null);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddCityAsync(string name);
        Task<List<DictionaryDTO>> GetSkillsAsync(string? search = null);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddSkillAsync(string name);
        Task<List<DictionaryDTO>> GetPositionsAsync(string? search = null);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> AddPositionAsync(string name);
        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateCityAsync(int id, string name);
        Task<(bool Success, string? Error)> DeleteCityAsync(int id);

        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdateSkillAsync(int id, string name);
        Task<(bool Success, string? Error)> DeleteSkillAsync(int id);

        Task<(bool Success, string? Error, DictionaryDTO? Item)> UpdatePositionAsync(int id, string name);
        Task<(bool Success, string? Error)> DeletePositionAsync(int id);
    }
}
