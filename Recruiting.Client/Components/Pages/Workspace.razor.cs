using Microsoft.AspNetCore.Components;
using MudBlazor;
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Pages;

public partial class Workspace
{
    [Inject] private IVacancyServiceClient VacancyService { get; set; } = default!;
    [Inject] private IApplicationServiceClient ApplicationService { get; set; } = default!;
    [Inject] private ICandidateServiceClient CandidateService { get; set; } = default!;
    [Inject] private IDictionaryServiceClient DictionaryService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [SupplyParameterFromQuery(Name = "vacId")]
    public string? VacancyId { get; set; }

    private bool _isLoading = true;
    private string _search = "";
    private string _currentStatus = "all"; // "all" или Id статуса в виде строки
    private int _selectedCandId = 0;
    private string _selectedVacId = "";

    // Данные всех справочников
    private ReferenceDataDTO? _references;
    private List<VacancyDTO> _vacancies = new();

    // Список вкладок (строятся динамически из FunnelStatuses)
    private List<(string Key, string Title)> _statusTabs = new()
    {
        ("all", "Все")
    };

    // Доступные этапы воронки из справочника
    public IReadOnlyList<DictionaryDTO> FunnelStages => _references?.FunnelStatuses ?? (IReadOnlyList<DictionaryDTO>)Array.Empty<DictionaryDTO>();

    public class CandModel
    {
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        public string Position { get; set; } = "";
        public int Salary { get; set; }
        public string Name { get; set; } = "";
        public string City { get; set; } = "";
        public string Age { get; set; } = "";
        public string Status => StatusId.ToString();
        public int StatusId { get; set; }
        public string StatusName { get; set; } = "";
        public string LastJob { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Telegram { get; set; } = "";
        public string Experience { get; set; } = "";
        public string[] Skills { get; set; } = Array.Empty<string>();
    }

    private List<CandModel> _list = new();

    private IEnumerable<CandModel> FilteredList =>
        _list.Where(c =>
            (_currentStatus == "all" || c.Status == _currentStatus) &&
            (string.IsNullOrWhiteSpace(_search) ||
             c.Name.Contains(_search, StringComparison.OrdinalIgnoreCase) ||
             c.Position.Contains(_search, StringComparison.OrdinalIgnoreCase)));

    private CandModel? CurrentCand => _list.FirstOrDefault(c => c.Id == _selectedCandId);

    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        _isLoading = true;
        try
        {
            // 1. Получаем справочники с бэкенда
            _references = await DictionaryService.GetBootstrapAsync();

            // Динамически заполняем вкладки статусов воронки
            _statusTabs = new List<(string Key, string Title)> { ("all", "Все") };
            if (_references?.FunnelStatuses != null)
            {
                foreach (var stage in _references.FunnelStatuses)
                {
                    _statusTabs.Add((stage.Id.ToString(), stage.Name));
                }
            }

            // 2. Получаем вакансии
            var vacResult = await VacancyService.GetVacanciesAsync(new VacancyFilter { PageSize = 100 });
            _vacancies = vacResult?.Items ?? new List<VacancyDTO>();

            if (!string.IsNullOrEmpty(VacancyId) && _vacancies.Any(v => v.Id.ToString() == VacancyId))
            {
                _selectedVacId = VacancyId;
            }
            else
            {
                _selectedVacId = _vacancies.FirstOrDefault()?.Id.ToString() ?? "";
            }

            // 3. Загружаем отклики по вакансии
            if (int.TryParse(_selectedVacId, out var currentVacId))
            {
                await LoadCandidatesForVacancyAsync(currentVacId);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка загрузки данных: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task HandleVacancyChanged(ChangeEventArgs e)
    {
        _selectedVacId = e.Value?.ToString() ?? "";
        if (int.TryParse(_selectedVacId, out var vacId))
        {
            await LoadCandidatesForVacancyAsync(vacId);
        }
    }

    private async Task LoadCandidatesForVacancyAsync(int vacancyId)
    {
        try
        {
            var appsResult = await ApplicationService.GetApplicationsAsync(new ApplicationFilter(
                VacancyId: vacancyId,
                CandidateId: null,
                StatusId: null,
                Page: 1,
                PageSize: 100));

            var applications = appsResult?.Items ?? new List<ApplicationDTO>();
            var newCandidatesList = new List<CandModel>();

            foreach (var app in applications)
            {
                var candidate = await CandidateService.GetByIdAsync(app.CandidateId);
                var fullName = candidate != null
                    ? $"{candidate.Surname} {candidate.Name} {candidate.Patronymic}".Trim()
                    : app.CandidateName;

                // Название статуса берем из справочника FunnelStatuses
                var statusTitle = _references?.FunnelStatuses?.FirstOrDefault(s => s.Id == app.ApplicationStatusId)?.Name
                                  ?? app.ApplicationStatusName;

                newCandidatesList.Add(new CandModel
                {
                    Id = app.CandidateId,
                    ApplicationId = app.Id,
                    Position = candidate?.PositionName ?? app.VacancyTitle,
                    Salary = 0,
                    Name = fullName,
                    City = candidate?.CityName ?? "Город не указан",
                    Age = "",
                    StatusId = app.ApplicationStatusId,
                    StatusName = statusTitle,
                    LastJob = candidate?.SourceName ?? "Прямой отклик",
                    Phone = candidate?.Phone ?? "—",
                    Telegram = candidate?.Email ?? "—",
                    Experience = candidate?.ExperienceYears.HasValue == true ? $"{candidate.ExperienceYears} лет" : "Без опыта",
                    Skills = candidate?.Skills?.Select(s => s.SkillName).ToArray() ?? Array.Empty<string>()
                });
            }

            _list = newCandidatesList;
            _selectedCandId = _list.FirstOrDefault()?.Id ?? 0;
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка загрузки кандидатов: {ex.Message}", Severity.Error);
        }
    }

    // Перевод кандидата на любой этап из справочника
    private async Task ChangeCandidateStageAsync(CandModel cand, int targetStageId)
    {
        var (success, error) = await ApplicationService.ChangeStageAsync(cand.ApplicationId, new ChangeStageRequest(targetStageId));
        if (success)
        {
            cand.StatusId = targetStageId;
            cand.StatusName = _references?.FunnelStatuses?.FirstOrDefault(s => s.Id == targetStageId)?.Name ?? "Обновлен";
            Snackbar.Add($"Кандидат переведен на этап: {cand.StatusName}", Severity.Success);
            StateHasChanged();
        }
        else
        {
            Snackbar.Add(error ?? "Не удалось сменить этап", Severity.Error);
        }
    }

    // Быстрый отказ (находит ID статуса со словом 'Отказ' из справочника)
    private async Task RejectCandidateAsync(CandModel cand)
    {
        var rejectStage = _references?.FunnelStatuses?.FirstOrDefault(s => s.Name.Contains("Отказ", StringComparison.OrdinalIgnoreCase));
        if (rejectStage != null)
        {
            await ChangeCandidateStageAsync(cand, rejectStage.Id);
        }
        else
        {
            Snackbar.Add("Статус отказа не найден в справочнике", Severity.Warning);
        }
    }

    private string GetStatusName(string status)
    {
        if (int.TryParse(status, out var id))
        {
            var found = _references?.FunnelStatuses?.FirstOrDefault(s => s.Id == id);
            if (found != null) return found.Name;
        }
        return status;
    }

    private string GetResponsesWord(int count)
    {
        var mod10 = count % 10;
        var mod100 = count % 100;
        if (mod10 == 1 && mod100 != 11) return "отклик";
        if (mod10 >= 2 && mod10 <= 4 && (mod100 < 10 || mod100 >= 20)) return "отклика";
        return "откликов";
    }
}