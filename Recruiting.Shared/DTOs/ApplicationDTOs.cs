using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record ApplicationDto(
        int Id,
        int VacancyId,
        string VacancyTitle,
        int CandidateId,
        string CandidateName,
        int ApplicationStatusId,
        string ApplicationStatusName,
        decimal? Score,
        decimal? SkillsScore,
        decimal? ExperienceScore,
        decimal? EducationScore,
        string? RejectReason,
        DateTime CreatedAt,
        DateTime UpdatedAt);

    public record CreateApplicationRequest(int VacancyId, int CandidateId, int? ApplicationStatusId = null);

    public record UpdateApplicationRequest(
        int? ApplicationStatusId,
        decimal? Score,
        decimal? SkillsScore,
        decimal? ExperienceScore,
        decimal? EducationScore,
        string? RejectReason);

    /// <summary>Смена этапа воронки. Пишется в StatusHistory.</summary>
    public record ChangeStageRequest(int ToStatusId, string? Comment = null);

    public record StatusHistoryDto(
        int Id,
        int ApplicationId,
        int? FromStatusId,
        string? FromStatusName,
        int ToStatusId,
        string ToStatusName,
        string? Comment,
        DateTime ChangedAt);

    public record ApplicationFilter(
        int? VacancyId,
        int? CandidateId,
        int? StatusId,
        int Page = 1,
        int PageSize = 20);
}
