using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;

namespace Recruiting.Services
{
    public class ApplicationService: IApplicationService
    {
        private readonly DataBaseContext _db;
        private readonly ICurrentUserService _currentUser;
        public ApplicationService(DataBaseContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }
        public async Task<PagedResult<ApplicationDTO>> GetApplicationsAsync(ApplicationFilter filter)
        {
            var query = _db.Applications
                .AsNoTracking()
                .Include(a => a.Vacancy)
                .Include(a => a.Candidate)
                .Include(a => a.ApplicationStatus)
                .AsQueryable();
            // Изоляция: HR видит отклики по своим вакансиям
            if (!_currentUser.IsAdmin)
            {
                var userId = _currentUser.UserId ?? 0;
                query = query.Where(a => a.Vacancy.OwnerId == userId);
            }
            // Фильтрация
            if (filter.VacancyId.HasValue)
                query = query.Where(a => a.VacancyId == filter.VacancyId.Value);
            if (filter.CandidateId.HasValue)
                query = query.Where(a => a.CandidateId == filter.CandidateId.Value);
            if (filter.StatusId.HasValue)
                query = query.Where(a => a.ApplicationStatusId == filter.StatusId.Value);
            var total = await query.CountAsync();
            var page = filter.Page < 1 ? 1 : filter.Page;
            var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;
            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(a => MapToDto(a))
                .ToListAsync();
            return new PagedResult<ApplicationDTO>(items, total, page, pageSize);
        }
        public async Task<ApplicationDTO?> GetApplicationByIdAsync(int id)
        {
            var app = await _db.Applications
                .AsNoTracking()
                .Include(a => a.Vacancy)
                .Include(a => a.Candidate)
                .Include(a => a.ApplicationStatus)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (app == null)
                return null;
            if (!_currentUser.IsAdmin && app.Vacancy.OwnerId != _currentUser.UserId)
                return null;
            return MapToDto(app);
        }
        public async Task<List<StatusHistoryDTO>> GetHistoryAsync(int applicationId)
        {
            var app = await _db.Applications
                .AsNoTracking()
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(a => a.Id == applicationId);
            if (app == null)
                return new List<StatusHistoryDTO>();
            if (!_currentUser.IsAdmin && app.Vacancy.OwnerId != _currentUser.UserId)
                return new List<StatusHistoryDTO>();
            return await _db.StatusHistories
                .AsNoTracking()
                .Where(h => h.ApplicationId == applicationId)
                .Include(h => h.FromStatus)
                .Include(h => h.ToStatus)
                .OrderByDescending(h => h.ChangedAt)
                .Select(h => new StatusHistoryDTO(
                    h.Id,
                    h.ApplicationId,
                    h.FromStatusId,
                    h.FromStatus != null ? h.FromStatus.Name : null,
                    h.ToStatusId,
                    h.ToStatus.Name,
                    h.Comment,
                    h.ChangedAt
                ))
                .ToListAsync();
        }
        public async Task<(bool Success, string? Error, int? Id)> CreateApplicationAsync(CreateApplicationRequest request)
        {
            if (!_currentUser.UserId.HasValue)
                return (false, "Пользователь не авторизован", null);
            var vacancy = await _db.Vacancies.FindAsync(request.VacancyId);
            if (vacancy == null)
                return (false, "Вакансия не найдена", null);
            if (!_currentUser.IsAdmin && vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на добавление отклика к этой вакансии", null);
            var candidateExists = await _db.Candidates.AnyAsync(c => c.Id == request.CandidateId);
            if (!candidateExists)
                return (false, "Кандидат не найден", null);
            // Проверка на дубликат отклика
            var alreadyApplied = await _db.Applications.AnyAsync(a =>
                a.VacancyId == request.VacancyId && a.CandidateId == request.CandidateId);
            if (alreadyApplied)
                return (false, "Этот кандидат уже привязан к данной вакансии", null);
            var statusId = request.ApplicationStatusId > 0
                ? request.ApplicationStatusId.Value
                : (int)FunnelStatusEnum.New;
            var now = DateTime.UtcNow;
            var app = new Application
            {
                VacancyId = request.VacancyId,
                CandidateId = request.CandidateId,
                ApplicationStatusId = statusId,
                CreatedAt = now,
                UpdatedAt = now
            };
            // Записываем начальный статус в историю
            app.StatusHistories.Add(new StatusHistory
            {
                FromStatusId = null,
                ToStatusId = statusId,
                Comment = "Создание отклика",
                ChangedAt = now
            });
            _db.Applications.Add(app);
            await _db.SaveChangesAsync();
            return (true, null, app.Id);
        }
        public async Task<(bool Success, string? Error)> ChangeStageAsync(int id, ChangeStageRequest request)
        {
            var app = await _db.Applications
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (app == null)
                return (false, "Отклик не найден");
            if (!_currentUser.IsAdmin && app.Vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на изменение этапа этого отклика");
            if (app.ApplicationStatusId == request.ToStatusId)
                return (true, null); // Статус не изменился
            var oldStatusId = app.ApplicationStatusId;
            var now = DateTime.UtcNow;
            app.ApplicationStatusId = request.ToStatusId;
            app.UpdatedAt = now;
            // Если статус «Отказ», фиксируем причину
            if (request.ToStatusId == (int)FunnelStatusEnum.Rejected && !string.IsNullOrWhiteSpace(request.Comment))
            {
                app.RejectReason = request.Comment;
            }
            // Логируем переход в историю
            var historyEntry = new StatusHistory
            {
                ApplicationId = app.Id,
                FromStatusId = oldStatusId,
                ToStatusId = request.ToStatusId,
                Comment = request.Comment,
                ChangedAt = now
            };
            _db.StatusHistories.Add(historyEntry);
            await _db.SaveChangesAsync();
            return (true, null);
        }
        public async Task<(bool Success, string? Error)> DeleteApplicationAsync(int id)
        {
            var app = await _db.Applications
                .Include(a => a.Vacancy)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (app == null)
                return (false, "Отклик не найден");
            if (!_currentUser.IsAdmin && app.Vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на удаление этого отклика");
            _db.Applications.Remove(app);
            await _db.SaveChangesAsync();
            return (true, null);
        }
        private static ApplicationDTO MapToDto(Application a)
        {
            var candidateFullName = string.Join(" ", new[] { a.Candidate?.Surname, a.Candidate?.Name, a.Candidate?.Patronymic }
                .Where(s => !string.IsNullOrWhiteSpace(s)));
            if (string.IsNullOrWhiteSpace(candidateFullName))
                candidateFullName = a.Candidate?.Email ?? $"Кандидат #{a.CandidateId}";
            return new ApplicationDTO(
                a.Id,
                a.VacancyId,
                a.Vacancy?.Title ?? string.Empty,
                a.CandidateId,
                candidateFullName,
                a.ApplicationStatusId,
                a.ApplicationStatus?.Name ?? string.Empty,
                a.Score,
                a.SkillsScore,
                a.ExperienceScore,
                a.EducationScore,
                a.RejectReason,
                a.CreatedAt,
                a.UpdatedAt
            );
        }
    }
}
