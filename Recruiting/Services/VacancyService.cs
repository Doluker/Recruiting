using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;

namespace Recruiting.Services
{
    public class VacancyService:IVacancyService
    {
        private readonly DataBaseContext _db;
        private readonly ICurrentUserService _currentUser;
        public VacancyService(DataBaseContext db, ICurrentUserService currentUser)
        {
            _db = db;
            _currentUser = currentUser;
        }
        public async Task<PagedResult<VacancyDTO>> GetVacanciesAsync(VacancyFilter filter)
        {
            var query = _db.Vacancies
                .AsNoTracking()
                .Include(v => v.VacancyStatus)
                .Include(v => v.City)
                .Include(v => v.Source)
                .Include(v => v.Education)
                .Include(v => v.VacancySkills).ThenInclude(vs => vs.Skill)
                .Include(v => v.Applications)
                .AsQueryable();
            // Изоляция данных: HR видит только свои, Admin — все
            if (!_currentUser.IsAdmin)
            {
                var currentUserId = _currentUser.UserId ?? 0;
                query = query.Where(v => v.OwnerId == currentUserId);
            }
            // Фильтрация
            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                var search = filter.Search.Trim().ToLower();
                query = query.Where(v => v.Title.ToLower().Contains(search) ||
                                         (v.Description != null && v.Description.ToLower().Contains(search)));
            }
            if (filter.StatusId.HasValue)
            {
                query = query.Where(v => v.VacancyStatusId == filter.StatusId.Value);
            }
            if (filter.CityId.HasValue)
            {
                query = query.Where(v => v.CityId == filter.CityId.Value);
            }
            if (filter.SourceId.HasValue)
            {
                query = query.Where(v => v.SourceId == filter.SourceId.Value);
            }
            if (filter.OnlyActive == true)
            {
                query = query.Where(v => v.VacancyStatusId == (int)VacancyStatusEnum.Active);
            }
            // Подсчёт общего количества
            var total = await query.CountAsync();
            // Сортировка
            query = (filter.Sort?.ToLower(), filter.Direction) switch
            {
                ("title", SortDirectionEnum.Asc) => query.OrderBy(v => v.Title),
                ("title", SortDirectionEnum.Desc) => query.OrderByDescending(v => v.Title),
                ("salary", SortDirectionEnum.Asc) => query.OrderBy(v => v.SalaryFrom),
                ("salary", SortDirectionEnum.Desc) => query.OrderByDescending(v => v.SalaryTo),
                (_, SortDirectionEnum.Asc) => query.OrderBy(v => v.CreatedAt),
                _ => query.OrderByDescending(v => v.CreatedAt)
            };
            // Постраничность
            var page = filter.Page < 1 ? 1 : filter.Page;
            var pageSize = filter.PageSize < 1 ? 20 : filter.PageSize;
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(v => MapToDto(v))
                .ToListAsync();
            return new PagedResult<VacancyDTO>(items, total, page, pageSize);
        }
        public async Task<VacancyDTO?> GetVacancyByIdAsync(int id)
        {
            var vacancy = await _db.Vacancies
                .AsNoTracking()
                .Include(v => v.VacancyStatus)
                .Include(v => v.City)
                .Include(v => v.Source)
                .Include(v => v.Education)
                .Include(v => v.VacancySkills).ThenInclude(vs => vs.Skill)
                .Include(v => v.Applications)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy == null)
                return null;
            // Проверка прав доступа
            if (!_currentUser.IsAdmin && vacancy.OwnerId != _currentUser.UserId)
                return null;
            return MapToDto(vacancy);
        }
        public async Task<(bool Success, string? Error, int? Id)> CreateVacancyAsync(CreateVacancyRequest request)
        {
            if (!_currentUser.UserId.HasValue)
                return (false, "Пользователь не авторизован", null);
            if (string.IsNullOrWhiteSpace(request.Title))
                return (false, "Название вакансии обязательно для заполнения", null);
            var now = DateTime.UtcNow;
            var vacancy = new Vacancy
            {
                OwnerId = _currentUser.UserId.Value,
                Title = request.Title.Trim(),
                Description = request.Description?.Trim(),
                VacancyStatusId = request.VacancyStatusId > 0 ? request.VacancyStatusId : (int)VacancyStatusEnum.Draft,
                CityId = request.CityId,
                SourceId = request.SourceId > 0 ? request.SourceId : (int)SourceEnum.Manual,
                SalaryFrom = request.SalaryFrom,
                SalaryTo = request.SalaryTo,
                ExperienceYears = request.ExperienceYears,
                EducationId = request.EducationId,
                CreatedAt = now,
                UpdatedAt = now
            };
            if (request.Skills != null && request.Skills.Count > 0)
            {
                foreach (var s in request.Skills)
                {
                    vacancy.VacancySkills.Add(new VacancySkill
                    {
                        SkillId = s.SkillId,
                        IsRequired = s.IsRequired
                    });
                }
            }
            _db.Vacancies.Add(vacancy);
            await _db.SaveChangesAsync();
            return (true, null, vacancy.Id);
        }
        public async Task<(bool Success, string? Error)> UpdateVacancyAsync(int id, UpdateVacancyRequest request)
        {
            var vacancy = await _db.Vacancies
                .Include(v => v.VacancySkills)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (vacancy == null)
                return (false, "Вакансия не найдена");
            if (!_currentUser.IsAdmin && vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на редактирование этой вакансии");
            if (string.IsNullOrWhiteSpace(request.Title))
                return (false, "Название вакансии обязательно для заполнения");
            vacancy.Title = request.Title.Trim();
            vacancy.Description = request.Description?.Trim();
            vacancy.VacancyStatusId = request.VacancyStatusId;
            vacancy.CityId = request.CityId;
            vacancy.SourceId = request.SourceId;
            vacancy.SalaryFrom = request.SalaryFrom;
            vacancy.SalaryTo = request.SalaryTo;
            vacancy.ExperienceYears = request.ExperienceYears;
            vacancy.EducationId = request.EducationId;
            vacancy.UpdatedAt = DateTime.UtcNow;
            // Синхронизация навыков
            vacancy.VacancySkills.Clear();
            if (request.Skills != null)
            {
                foreach (var s in request.Skills)
                {
                    vacancy.VacancySkills.Add(new VacancySkill
                    {
                        VacancyId = vacancy.Id,
                        SkillId = s.SkillId,
                        IsRequired = s.IsRequired
                    });
                }
            }
            await _db.SaveChangesAsync();
            return (true, null);
        }
        public async Task<(bool Success, string? Error)> ChangeStatusAsync(int id, int statusId)
        {
            var vacancy = await _db.Vacancies.FindAsync(id);
            if (vacancy == null)
                return (false, "Вакансия не найдена");
            if (!_currentUser.IsAdmin && vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на изменение статуса этой вакансии");
            vacancy.VacancyStatusId = statusId;
            vacancy.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return (true, null);
        }
        public async Task<(bool Success, string? Error)> DeleteAsync(int id)
        {
            var vacancy = await _db.Vacancies.FindAsync(id);
            if (vacancy == null)
                return (false, "Вакансия не найдена");
            if (!_currentUser.IsAdmin && vacancy.OwnerId != _currentUser.UserId)
                return (false, "У вас нет прав на удаление этой вакансии");
            _db.Vacancies.Remove(vacancy);
            await _db.SaveChangesAsync();
            return (true, null);
        }
        private static VacancyDTO MapToDto(Vacancy v)
        {
            return new VacancyDTO(
                v.Id,
                v.Title,
                v.Description,
                v.VacancyStatusId,
                v.VacancyStatus?.Name ?? string.Empty,
                v.CityId,
                v.City?.Name,
                v.SourceId,
                v.Source?.Name ?? string.Empty,
                v.SalaryFrom,
                v.SalaryTo,
                v.ExperienceYears,
                v.EducationId,
                v.Education?.Name,
                v.OwnerId,
                v.CreatedAt,
                v.UpdatedAt,
                v.VacancySkills.Select(vs => new VacancySkillDTO(
                    vs.Id,
                    vs.SkillId,
                    vs.Skill?.Name ?? string.Empty,
                    vs.IsRequired
                )).ToList(),
                v.Applications?.Count ?? 0
            );
        }
    }
}
