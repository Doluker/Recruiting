using Microsoft.EntityFrameworkCore;
using Recruiting.Interfaces;
using Recruiting.Models;
using Recruiting.Shared.DTOs;

namespace Recruiting.Services;

public class DictionaryService : IDictionaryService
{
    private readonly DataBaseContext _db;

    public DictionaryService(DataBaseContext db)
    {
        _db = db;
    }

    public async Task<ReferenceDataDTO> GetBootstrapDataAsync()
    {
        var sources = await _db.Sources.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var cities = await _db.Cities.AsNoTracking().OrderBy(x => x.Name).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var educations = await _db.Educations.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var positions = await _db.Positions.AsNoTracking().OrderBy(x => x.Name).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var skills = await _db.Skills.AsNoTracking().OrderBy(x => x.Name).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var vacancyStatuses = await _db.VacancyStatuses.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var funnelStatuses = await _db.FunnelStatuses.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var letterKinds = await _db.LetterKinds.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var letterStatuses = await _db.LetterStatuses.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var parseJobStatuses = await _db.ParseJobStatuses.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var resumeContentTypes = await _db.ResumeContentTypes.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();
        var roles = await _db.Roles.AsNoTracking().OrderBy(x => x.Id).Select(x => new DictionaryDTO(x.Id, x.Name)).ToListAsync();

        var plans = await _db.Plans.AsNoTracking().OrderBy(x => x.Id).Select(p => new PlanDTO(
            p.Id,
            p.Name,
            p.PriceMonthly,
            p.MaxUploadsPerMonth,
            p.MaxActiveVacancies,
            p.CanParse,
            p.CanExportExcel,
            p.HasFullDashboard
        )).ToListAsync();

        var currencies = await _db.Currencies.AsNoTracking().OrderBy(x => x.Id).Select(c => new CurrencyDTO(
            c.Id,
            c.Name,
            c.Symbol
        )).ToListAsync();

        return new ReferenceDataDTO(
            sources,
            cities,
            educations,
            positions,
            skills,
            vacancyStatuses,
            funnelStatuses,
            letterKinds,
            letterStatuses,
            parseJobStatuses,
            resumeContentTypes,
            roles,
            plans,
            currencies
        );
    }

    public async Task<List<DictionaryDTO>> GetCitiesAsync(string? search = null)
    {
        var query = _db.Cities.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(c => c.Name.ToLower().Contains(s));
        }
        return await query.OrderBy(c => c.Name).Select(c => new DictionaryDTO(c.Id, c.Name)).ToListAsync();
    }

    public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddCityAsync(string name)
    {
        var cleanName = name.Trim();
        if (string.IsNullOrWhiteSpace(cleanName))
            return (false, "Название города не может быть пустым", null);

        var existing = await _db.Cities.FirstOrDefaultAsync(c => c.Name.ToLower() == cleanName.ToLower());
        if (existing != null)
            return (true, null, new DictionaryDTO(existing.Id, existing.Name));

        var city = new City { Name = cleanName };
        _db.Cities.Add(city);
        await _db.SaveChangesAsync();

        return (true, null, new DictionaryDTO(city.Id, city.Name));
    }

    public async Task<List<DictionaryDTO>> GetSkillsAsync(string? search = null)
    {
        var query = _db.Skills.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(s1 => s1.Name.ToLower().Contains(s));
        }
        return await query.OrderBy(s => s.Name).Select(s => new DictionaryDTO(s.Id, s.Name)).ToListAsync();
    }

    public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddSkillAsync(string name)
    {
        var cleanName = name.Trim();
        if (string.IsNullOrWhiteSpace(cleanName))
            return (false, "Название навыка не может быть пустым", null);

        var existing = await _db.Skills.FirstOrDefaultAsync(s => s.Name.ToLower() == cleanName.ToLower());
        if (existing != null)
            return (true, null, new DictionaryDTO(existing.Id, existing.Name));

        var skill = new Skill { Name = cleanName };
        _db.Skills.Add(skill);
        await _db.SaveChangesAsync();

        return (true, null, new DictionaryDTO(skill.Id, skill.Name));
    }

    public async Task<List<DictionaryDTO>> GetPositionsAsync(string? search = null)
    {
        var query = _db.Positions.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(s));
        }
        return await query.OrderBy(p => p.Name).Select(p => new DictionaryDTO(p.Id, p.Name)).ToListAsync();
    }

    public async Task<(bool Success, string? Error, DictionaryDTO? Item)> AddPositionAsync(string name)
    {
        var cleanName = name.Trim();
        if (string.IsNullOrWhiteSpace(cleanName))
            return (false, "Название должности не может быть пустым", null);

        var existing = await _db.Positions.FirstOrDefaultAsync(p => p.Name.ToLower() == cleanName.ToLower());
        if (existing != null)
            return (true, null, new DictionaryDTO(existing.Id, existing.Name));

        var pos = new Position { Name = cleanName };
        _db.Positions.Add(pos);
        await _db.SaveChangesAsync();

        return (true, null, new DictionaryDTO(pos.Id, pos.Name));
    }
}