using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;

namespace Recruiting.Services;

public class CandidateService : ICandidateService
{
    private readonly DataBaseContext _db;
    private readonly ICurrentUserService _currentUser;

    public CandidateService(DataBaseContext db, ICurrentUserService currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }

    public async Task<PagedResult<CandidateDTO>> GetCandidatesAsync(CandidateFilter filter)
    {
        var query = _db.Candidates
            .AsNoTracking()
            .Include(c => c.City)
            .Include(c => c.Position)
            .Include(c => c.Education)
            .Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Include(c => c.Applications)
            .Include(c => c.ResumeFiles)
            .AsQueryable();

        // 1. Изоляция: HR видит своих кандидатов (или откликнувшихся на его вакансии)
        if (!_currentUser.IsAdmin)
        {
            var userId = _currentUser.UserId ?? 0;
            query = query.Where(c => c.OwnerId == userId || c.Applications.Any(a => a.Vacancy.OwnerId == userId));
        }

        // 2. Поиск по ФИО, Email, телефону
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            var s = filter.Search.Trim().ToLower();
            query = query.Where(c =>
                (c.Surname != null && c.Surname.ToLower().Contains(s)) ||
                (c.Name != null && c.Name.ToLower().Contains(s)) ||
                (c.Patronymic != null && c.Patronymic.ToLower().Contains(s)) ||
                (c.Email != null && c.Email.ToLower().Contains(s)) ||
                (c.Phone != null && c.Phone.Contains(s))
            );
        }

        // 3. Фильтры из твоего CandidateFilter DTO
        if (filter.CityId.HasValue)
            query = query.Where(c => c.CityId == filter.CityId.Value);

        if (filter.PositionId.HasValue)
            query = query.Where(c => c.PositionId == filter.PositionId.Value);

        if (filter.SourceId.HasValue)
            query = query.Where(c => c.SourceId == filter.SourceId.Value);

        if (filter.EducationId.HasValue)
            query = query.Where(c => c.EducationId == filter.EducationId.Value);

        if (filter.SkillId.HasValue)
            query = query.Where(c => c.CandidateSkills.Any(cs => cs.SkillId == filter.SkillId.Value));

        // 4. Подсчет общего количества
        var total = await query.CountAsync();

        // 5. Сортировка
        query = (filter.Sort?.ToLower(), filter.Direction) switch
        {
            ("name", SortDirectionEnum.Asc) => query.OrderBy(c => c.Surname).ThenBy(c => c.Name),
            ("name", SortDirectionEnum.Desc) => query.OrderByDescending(c => c.Surname).ThenByDescending(c => c.Name),
            ("experience", SortDirectionEnum.Asc) => query.OrderBy(c => c.ExperienceYears),
            ("experience", SortDirectionEnum.Desc) => query.OrderByDescending(c => c.ExperienceYears),
            (_, SortDirectionEnum.Asc) => query.OrderBy(c => c.CreatedAt),
            _ => query.OrderByDescending(c => c.CreatedAt)
        };

        // 6. Постраничность
        var page = filter.Page < 1 ? 1 : filter.Page;
        var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => MapToDto(c))
            .ToListAsync();

        return new PagedResult<CandidateDTO>(items, total, page, pageSize);
    }

    public async Task<CandidateDTO?> GetCandidateByIdAsync(int id)
    {
        var candidate = await _db.Candidates
            .AsNoTracking()
            .Include(c => c.City)
            .Include(c => c.Position)
            .Include(c => c.Education)
            .Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .Include(c => c.Applications)
            .Include(c => c.ResumeFiles)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (candidate == null)
            return null;

        if (!_currentUser.IsAdmin && candidate.OwnerId != _currentUser.UserId &&
            !candidate.Applications.Any(a => a.Vacancy.OwnerId == _currentUser.UserId))
        {
            return null;
        }

        return MapToDto(candidate);
    }

    public async Task<(bool Success, string? Error, int? Id)> CreateCandidateAsync(CreateCandidateRequest request)
    {
        if (!_currentUser.UserId.HasValue)
            return (false, "Пользователь не авторизован", null);

        var now = DateTime.UtcNow;

        var candidate = new Candidate
        {
            OwnerId = _currentUser.UserId.Value,
            Surname = request.Surname?.Trim(),
            Name = request.Name?.Trim(),
            Patronymic = request.Patronymic?.Trim(),
            Email = request.Email?.Trim().ToLowerInvariant(),
            Phone = request.Phone?.Trim(),
            CityId = request.CityId,
            PositionId = request.PositionId,
            ExperienceYears = request.ExperienceYears,
            EducationId = request.EducationId,
            SourceId = request.SourceId > 0 ? request.SourceId : (int)SourceEnum.Manual,
            ExternalId = request.ExternalId,
            SourceUrl = request.SourceUrl,
            CreatedAt = now,
            UpdatedAt = now
        };

        if (request.Skills != null)
        {
            foreach (var s in request.Skills)
            {
                candidate.CandidateSkills.Add(new CandidateSkill
                {
                    SkillId = s.SkillId,
                    Years = s.Years
                });
            }
        }

        _db.Candidates.Add(candidate);
        await _db.SaveChangesAsync();

        return (true, null, candidate.Id);
    }

    public async Task<(bool Success, string? Error)> UpdateCandidateAsync(int id, UpdateCandidateRequest request)
    {
        var candidate = await _db.Candidates
            .Include(c => c.CandidateSkills)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (candidate == null)
            return (false, "Кандидат не найден");

        if (!_currentUser.IsAdmin && candidate.OwnerId != _currentUser.UserId)
            return (false, "У вас нет прав на редактирование этого кандидата");

        candidate.Surname = request.Surname?.Trim();
        candidate.Name = request.Name?.Trim();
        candidate.Patronymic = request.Patronymic?.Trim();
        candidate.Email = request.Email?.Trim().ToLowerInvariant();
        candidate.Phone = request.Phone?.Trim();
        candidate.CityId = request.CityId;
        candidate.PositionId = request.PositionId;
        candidate.ExperienceYears = request.ExperienceYears;
        candidate.EducationId = request.EducationId;
        candidate.ExternalId = request.ExternalId;
        candidate.SourceUrl = request.SourceUrl;
        candidate.UpdatedAt = DateTime.UtcNow;

        // Синхронизация навыков
        candidate.CandidateSkills.Clear();
        if (request.Skills != null)
        {
            foreach (var s in request.Skills)
            {
                candidate.CandidateSkills.Add(new CandidateSkill
                {
                    CandidateId = candidate.Id,
                    SkillId = s.SkillId,
                    Years = s.Years
                });
            }
        }

        await _db.SaveChangesAsync();
        return (true, null);
    }

    public async Task<(bool Success, string? Error)> DeleteCandidateAsync(int id)
    {
        var candidate = await _db.Candidates.FindAsync(id);
        if (candidate == null)
            return (false, "Кандидат не найден");

        if (!_currentUser.IsAdmin && candidate.OwnerId != _currentUser.UserId)
            return (false, "У вас нет прав на удаление этого кандидата");

        _db.Candidates.Remove(candidate);
        await _db.SaveChangesAsync();
        return (true, null);
    }

    private static CandidateDTO MapToDto(Candidate c)
    {
        return new CandidateDTO(
            c.Id,
            c.OwnerId,
            c.Surname,
            c.Name,
            c.Patronymic,
            c.Email,
            c.Phone,
            c.CityId,
            c.City?.Name,
            c.PositionId,
            c.Position?.Name,
            c.ExperienceYears,
            c.EducationId,
            c.Education?.Name,
            c.SourceId,
            GetSourceName(c.SourceId),
            c.ExternalId,
            c.SourceUrl,
            c.CreatedAt,
            c.UpdatedAt,
            c.CandidateSkills.Select(cs => new CandidateSkillDTO(
                cs.Id,
                cs.SkillId,
                cs.Skill?.Name ?? string.Empty,
                cs.Years
            )).ToList(),
            c.Applications?.Count ?? 0,
            c.ResumeFiles?.Count ?? 0
        );
    }

    private static string GetSourceName(int sourceId) => (SourceEnum)sourceId switch
    {
        SourceEnum.Manual => "Вручную",
        SourceEnum.Hh => "hh.ru",
        SourceEnum.HabrCareer => "Habr Career",
        SourceEnum.Nejkareru => "Nejkareru",
        _ => "Неизвестно"
    };
}