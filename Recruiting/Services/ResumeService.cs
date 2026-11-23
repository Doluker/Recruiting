using System.Security.Cryptography;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;

namespace Recruiting.Services;

public class ResumeService : IResumeService
{
    private readonly DataBaseContext _db;
    private readonly IStorageService _storage;
    private readonly ICurrentUserService _currentUser;

    public ResumeService(DataBaseContext db, IStorageService storage, ICurrentUserService currentUser)
    {
        _db = db;
        _storage = storage;
        _currentUser = currentUser;
    }

    public async Task<UploadResumeResponse> UploadResumeAsync(IFormFile file, CancellationToken ct = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Файл не выбран или пуст");

        var ownerId = _currentUser.UserId ?? throw new UnauthorizedAccessException();

        // 1. Вычисляем SHA-256 хеш файла для дедупликации
        string hash;
        using (var hashAlgorithm = SHA256.Create())
        await using (var stream = file.OpenReadStream())
        {
            var hashBytes = await hashAlgorithm.ComputeHashAsync(stream, ct);
            hash = Convert.ToHexString(hashBytes).ToLowerInvariant();
        }

        // 2. Проверяем дубликат у текущего рекрутера
        var existing = await _db.ResumeFiles
            .Include(r => r.ParseJob)
            .ThenInclude(pj => pj!.ParseJobStatus)
            .FirstOrDefaultAsync(r => r.OwnerId == ownerId && r.FileHash == hash, ct);

        if (existing != null)
        {
            return new UploadResumeResponse(
                ResumeFileId: existing.Id,
                IsDuplicate: true,
                ParseJobId: existing.ParseJob?.Id,
                ParseJobStatus: existing.ParseJob?.ParseJobStatus.Name ?? "В обработке",
                Message: "Данный файл резюме уже был загружен ранее."
            );
        }

        // 3. Загружаем файл в MinIO
        string objectKey;
        await using (var stream = file.OpenReadStream())
        {
            objectKey = await _storage.UploadAsync(stream, file.FileName, file.ContentType, file.Length, ct);
        }

        // 4. Определяем ContentTypeId (например, PDF = 1)
        int contentTypeId = file.ContentType.ToLowerInvariant() switch
        {
            "application/pdf" => (int)ResumeContentTypeEnum.Pdf,
            "application/msword" => (int)ResumeContentTypeEnum.Doc,
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => (int)ResumeContentTypeEnum.Docx,
            _ => (int)ResumeContentTypeEnum.Pdf
        };

        // 5. Записываем в БД файл и ставим задачу в очередь ParseJob
        var resumeFile = new ResumeFile
        {
            OwnerId = ownerId,
            OriginalFileName = file.FileName,
            ContentTypeId = contentTypeId,
            FileHash = hash,
            FileSize = file.Length,
            ObjectKey = objectKey,
            UploadedAt = DateTime.UtcNow
        };

        _db.ResumeFiles.Add(resumeFile);
        await _db.SaveChangesAsync(ct);

        var parseJob = new ParseJob
        {
            ResumeFileId = resumeFile.Id,
            ParseJobStatusId = (int)ParseJobStatusEnum.Pending,
            Attempts = 0,
            CreatedAt = DateTime.UtcNow
        };

        _db.ParseJobs.Add(parseJob);
        await _db.SaveChangesAsync(ct);

        return new UploadResumeResponse(
            ResumeFileId: resumeFile.Id,
            IsDuplicate: false,
            ParseJobId: parseJob.Id,
            ParseJobStatus: "Ожидает",
            Message: "Файл успешно загружен и поставлен в очередь на обработку."
        );
    }

    public async Task<ResumeFileDTO?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        var file = await _db.ResumeFiles
            .Include(r => r.ContentType)
            .Include(r => r.ParseJob)
                .ThenInclude(pj => pj!.ParseJobStatus)
            .FirstOrDefaultAsync(r => r.Id == id && (_currentUser.IsAdmin || r.OwnerId == _currentUser.UserId), ct);

        if (file == null) return null;

        return new ResumeFileDTO(
            file.Id,
            file.OwnerId,
            file.CandidateId,
            file.OriginalFileName,
            file.ContentTypeId,
            file.ContentType.Name,
            file.FileSize,
            file.FileHash,
            file.UploadedAt,
            file.ProcessedAt,
            file.ExtractedText,
            file.ParseJob?.Id,
            file.ParseJob?.ParseJobStatus?.Name
        );
    }

    public async Task<(Stream stream, string contentType, string fileName)> DownloadResumeFileAsync(int id, CancellationToken ct = default)
    {
        var file = await _db.ResumeFiles
            .Include(r => r.ContentType)
            .FirstOrDefaultAsync(r => r.Id == id && (_currentUser.IsAdmin || r.OwnerId == _currentUser.UserId), ct);

        if (file == null) throw new KeyNotFoundException("Файл не найден");

        var stream = await _storage.DownloadAsync(file.ObjectKey, ct);
        return (stream, file.ContentType.Name, file.OriginalFileName);
    }
}