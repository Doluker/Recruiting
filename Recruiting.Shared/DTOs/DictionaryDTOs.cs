using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    /// <summary>Универсальный DTO для всех name-only справочников.</summary>
    public record DictionaryDto(int Id, string Name);

    public record CreateDictionaryItemRequest(string Name);

    public record UpdateDictionaryItemRequest(string Name);

    public record CurrencyDto(int Id, string Name, string Symbol);

    /// <summary>
    /// Один запрос → все справочники для выпадающих списков.
    /// Клиент грузит это один раз при старте и кэширует.
    /// </summary>
    public record ReferenceDataDto(
        List<DictionaryDto> Sources,
        List<DictionaryDto> Cities,
        List<DictionaryDto> Educations,
        List<DictionaryDto> Positions,
        List<DictionaryDto> Skills,
        List<DictionaryDto> VacancyStatuses,
        List<DictionaryDto> FunnelStatuses,
        List<DictionaryDto> LetterKinds,
        List<DictionaryDto> LetterStatuses,
        List<DictionaryDto> ParseJobStatuses,
        List<DictionaryDto> ResumeContentTypes,
        List<DictionaryDto> Roles,
        List<PlanDto> Plans,
        List<CurrencyDto> Currencies);
}
