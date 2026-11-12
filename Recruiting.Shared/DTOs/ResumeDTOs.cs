using System;
using System.Collections.Generic;
using System.Text;

namespace Recruiting.Shared.DTOs
{
    public record ResumeFileDTO(
    int Id,
    int OwnerId,
    int? CandidateId,
    string OriginalFileName,
    int ContentTypeId,
    string ContentTypeName,
    long FileSize,
    string FileHash,
    DateTime UploadedAt,
    DateTime? ProcessedAt,
    string? ExtractedText,
    int? ParseJobId,
    string? ParseJobStatusName);

    /// <summary>Ответ на загрузку файла.</summary>
    public record UploadResumeResponse(
        int ResumeFileId,
        bool IsDuplicate,
        int? ParseJobId,
        string? ParseJobStatus,
        string Message);

    public record ParseJobDTO(
        int Id,
        int ResumeFileId,
        int ParseJobStatusId,
        string ParseJobStatusName,
        int Attempts,
        string? Error,
        DateTime CreatedAt,
        DateTime? StartedAt,
        DateTime? LockedUntil,
        DateTime? FinishedAt);

    /// <summary>Результат парсинга — плоский, чтобы клиенту не тащить JSON.</summary>
    public record ParseJobResultDTO(
        int ResumeFileId,
        string? Surname,
        string? Name,
        string? Patronymic,
        string? Email,
        string? Phone,
        string? Position,
        string? City,
        string? Education,
        int? ExperienceYears,
        List<string> Skills,
        string? RawText);

    public record ResumeFileFilter(
        int? CandidateId,
        int? ParseJobStatusId,
        int Page = 1,
        int PageSize = 20);
}
