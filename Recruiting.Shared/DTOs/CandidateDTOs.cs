using Recruiting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record CandidateDTO(
    int Id,
    int OwnerId,
    string? Surname,
    string? Name,
    string? Patronymic,
    string? Email,
    string? Phone,
    int? CityId,
    string? CityName,
    int? PositionId,
    string? PositionName,
    int? ExperienceYears,
    int? EducationId,
    string? EducationName,
    int SourceId,
    string SourceName,
    string? ExternalId,
    string? SourceUrl,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<CandidateSkillDTO> Skills,
    int ApplicationsCount,
    int ResumeFilesCount);

    public record CandidateSkillDTO(int Id, int SkillId, string SkillName, int? Years);

    public record CandidateSkillRequest(int SkillId, int? Years = null);

    public record CreateCandidateRequest(
        string? Surname,
        string? Name,
        string? Patronymic,
        string? Email,
        string? Phone,
        int? CityId,
        int? PositionId,
        int? ExperienceYears,
        int? EducationId,
        int SourceId,
        string? ExternalId,
        string? SourceUrl,
        List<CandidateSkillRequest> Skills);

    public record UpdateCandidateRequest(
        string? Surname,
        string? Name,
        string? Patronymic,
        string? Email,
        string? Phone,
        int? CityId,
        int? PositionId,
        int? ExperienceYears,
        int? EducationId,
        string? ExternalId,
        string? SourceUrl,
        List<CandidateSkillRequest> Skills);

    /// <summary>Фильтры списка кандидатов.</summary>
    public record CandidateFilter(
        string? Search = null,
        int? CityId = null,
        int? PositionId = null,
        int? SourceId = null,
        int? SkillId = null,
        int? EducationId = null,
        int Page = 1,
        int PageSize = 20,
        string Sort = "createdAt",
        SortDirectionEnum Direction = SortDirectionEnum.Desc);

    /// <summary>
    /// Результат матчинга резюме на вакансию.
    /// Оценки 0..100 — как в CK_Applications_Score.
    /// </summary>
    public record CandidateMatchDTO(
        int CandidateId,
        string? Surname,
        string? Name,
        string? Email,
        string? Phone,
        decimal? SkillsScore,
        decimal? ExperienceScore,
        decimal? EducationScore,
        decimal? TotalScore,
        List<string> MatchedSkills,
        List<string> MissingSkills);

}
