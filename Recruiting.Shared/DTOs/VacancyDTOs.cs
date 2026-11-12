using Recruiting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record VacancyDTO(
    int Id,
    string Title,
    string? Description,
    int VacancyStatusId,
    string VacancyStatusName,
    int? CityId,
    string? CityName,
    int SourceId,
    string SourceName,
    int? SalaryFrom,
    int? SalaryTo,
    int? ExperienceYears,
    int? EducationId,
    string? EducationName,
    int OwnerId,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<VacancySkillDTO> Skills,
    int ApplicationsCount);

    public record VacancySkillDTO(int Id, int SkillId, string SkillName, bool IsRequired);

    public record CreateVacancyRequest(
        string Title,
        string? Description,
        int VacancyStatusId,
        int? CityId,
        int SourceId,
        int? SalaryFrom,
        int? SalaryTo,
        int? ExperienceYears,
        int? EducationId,
        List<VacancySkillRequest> Skills);

    public record UpdateVacancyRequest(
        string Title,
        string? Description,
        int VacancyStatusId,
        int? CityId,
        int SourceId,
        int? SalaryFrom,
        int? SalaryTo,
        int? ExperienceYears,
        int? EducationId,
        List<VacancySkillRequest> Skills);

    public record VacancySkillRequest(int SkillId, bool IsRequired = false);

    /// <summary>Фильтры и постраничность для списка вакансий.</summary>
    public record VacancyFilter(
        string? Search = null,
        int? StatusId = null,
        int? CityId = null,
        int? SourceId = null,
        bool? OnlyActive = null,
        int Page = 1,
        int PageSize = 20,
        string Sort = "createdAt",
        SortDirectionEnum Direction = SortDirectionEnum.Desc);

}
