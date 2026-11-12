using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    /// <summary>Универсальный DTO для всех name-only справочников.</summary>
    public record DictionaryDTO(int Id, string Name);

    public record CreateDictionaryItemRequest(string Name);

    public record UpdateDictionaryItemRequest(string Name);

    public record CurrencyDTO(int Id, string Name, string Symbol);

    /// <summary>
    /// Один запрос → все справочники для выпадающих списков.
    /// Клиент грузит это один раз при старте и кэширует.
    /// </summary>
    public record ReferenceDataDTO(
        List<DictionaryDTO> Sources,
        List<DictionaryDTO> Cities,
        List<DictionaryDTO> Educations,
        List<DictionaryDTO> Positions,
        List<DictionaryDTO> Skills,
        List<DictionaryDTO> VacancyStatuses,
        List<DictionaryDTO> FunnelStatuses,
        List<DictionaryDTO> LetterKinds,
        List<DictionaryDTO> LetterStatuses,
        List<DictionaryDTO> ParseJobStatuses,
        List<DictionaryDTO> ResumeContentTypes,
        List<DictionaryDTO> Roles,
        List<PlanDTO> Plans,
        List<CurrencyDTO> Currencies);
}
