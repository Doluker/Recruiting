using Microsoft.AspNetCore.Components;
using MudBlazor;
using Recruiting.Client.Components.Dialogs;
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Pages;

public partial class Admin
{
[Inject] private IDictionaryServiceClient DictionaryService { get; set; } = default!;
    [Inject] private IVacancyServiceClient VacancyService { get; set; } = default!;
    [Inject] private ICandidateServiceClient CandidateService { get; set; } = default!;
    [Inject] private IApplicationServiceClient ApplicationService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private IAuthServiceClient AuthService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    private bool _isLoading = true;
    private ReferenceDataDTO? _references;
    // Справочники
    private List<DictionaryDTO> _skills = new();
    private List<DictionaryDTO> _cities = new();
    private List<DictionaryDTO> _positions = new();
    private List<UserProfileDTO> _users = new();
    private string _skillSearch = "";
    private string _citySearch = "";
    private string _positionSearch = "";
    // Реальные вакансии, кандидаты и отклики
    private List<VacancyDTO> _vacancies = new();
    private List<CandidateDTO> _candidates = new();
    private List<ApplicationDTO> _applications = new();
    private string _vacancySearch = "";
    private string _candidateSearch = "";
    private string _applicationSearch = "";
    public IReadOnlyList<DictionaryDTO> FunnelStatuses => _references?.FunnelStatuses ?? (IReadOnlyList<DictionaryDTO>)Array.Empty<DictionaryDTO>();
    public IEnumerable<DictionaryDTO> FilteredSkills =>
        _skills.Where(s => string.IsNullOrWhiteSpace(_skillSearch) || s.Name.Contains(_skillSearch, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<DictionaryDTO> FilteredCities =>
        _cities.Where(c => string.IsNullOrWhiteSpace(_citySearch) || c.Name.Contains(_citySearch, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<DictionaryDTO> FilteredPositions =>
        _positions.Where(p => string.IsNullOrWhiteSpace(_positionSearch) || p.Name.Contains(_positionSearch, StringComparison.OrdinalIgnoreCase));
    public IEnumerable<VacancyDTO> FilteredVacancies =>
        _vacancies.Where(v => string.IsNullOrWhiteSpace(_vacancySearch) ||
            v.Title.Contains(_vacancySearch, StringComparison.OrdinalIgnoreCase) ||
            (v.CityName != null && v.CityName.Contains(_vacancySearch, StringComparison.OrdinalIgnoreCase)));
    public IEnumerable<CandidateDTO> FilteredCandidates =>
        _candidates.Where(c => string.IsNullOrWhiteSpace(_candidateSearch) ||
            $"{c.Surname} {c.Name} {c.Patronymic}".Contains(_candidateSearch, StringComparison.OrdinalIgnoreCase) ||
            (c.Email != null && c.Email.Contains(_candidateSearch, StringComparison.OrdinalIgnoreCase)) ||
            (c.PositionName != null && c.PositionName.Contains(_candidateSearch, StringComparison.OrdinalIgnoreCase)));
    public IEnumerable<ApplicationDTO> FilteredApplications =>
        _applications.Where(a => string.IsNullOrWhiteSpace(_applicationSearch) ||
            a.CandidateName.Contains(_applicationSearch, StringComparison.OrdinalIgnoreCase) ||
            a.VacancyTitle.Contains(_applicationSearch, StringComparison.OrdinalIgnoreCase));
    protected override async Task OnInitializedAsync()
    {
        await LoadDataAsync();
    }
    private async Task LoadDataAsync()
    {
        _isLoading = true;
        try
        {
            var refTask = DictionaryService.GetBootstrapAsync();
            var vacTask = VacancyService.GetVacanciesAsync(new VacancyFilter { Page = 1, PageSize = 100 });
            var candTask = CandidateService.GetCandidatesAsync(new CandidateFilter { Page = 1, PageSize = 100 });
            var appTask = ApplicationService.GetApplicationsAsync(new ApplicationFilter(null, null, null, 1, 100));
            await Task.WhenAll(refTask, vacTask, candTask, appTask);
            _references = await refTask;
            if (_references != null)
            {
                _skills = _references.Skills?.ToList() ?? new();
                _cities = _references.Cities?.ToList() ?? new();
                _positions = _references.Positions?.ToList() ?? new();
            }
            var vacResult = await vacTask;
            _vacancies = vacResult?.Items?.ToList() ?? new();
            var candResult = await candTask;
            _candidates = candResult?.Items?.ToList() ?? new();
            var appResult = await appTask;
            _applications = appResult?.Items?.ToList() ?? new();
            var users = await AuthService.GetAllUsersAsync();
            _users = users ?? new();
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

    // Добавление навыка через динамический диалог
    private async Task OpenAddSkillDialogAsync()
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
        {
            new()
            {
                Key = "name",
                Label = "Название навыка",
                Placeholder = "Например: PostgreSQL, Docker, Redis",
                HelperText = "Укажите точное наименование технологии или компетенции",
                FieldType = DynamicFormDialog.DynamicFieldType.Text,
                IsRequired = true
            }
        };

        var values = await ShowDynamicDialogAsync(
            title: "Новый навык",
            subtitle: "Добавление компетенции в глобальный справочник",
            icon: Icons.Material.Outlined.Psychology,
            fields: fields);

        if (values != null && values.TryGetValue("name", out var val) && val is string skillName && !string.IsNullOrWhiteSpace(skillName))
        {
            var (success, error, item) = await DictionaryService.AddSkillAsync(skillName.Trim());
            if (success && item != null)
            {
                _skills.Add(item);
                Snackbar.Add($"Навык «{item.Name}» успешно добавлен", Severity.Success);
            }
            else
            {
                Snackbar.Add(error ?? "Не удалось добавить навык", Severity.Error);
            }
        }
    }

    // Добавление города через динамический диалог
    private async Task OpenAddCityDialogAsync()
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
        {
            new()
            {
                Key = "name",
                Label = "Город / Локация",
                Placeholder = "Например: Новосибирск, Владивосток",
                HelperText = "Будет использоваться для гео-фильтрации кандидатов",
                FieldType = DynamicFormDialog.DynamicFieldType.Text,
                IsRequired = true
            }
        };

        var values = await ShowDynamicDialogAsync(
            title: "Новый город",
            subtitle: "Добавление локации в список городов",
            icon: Icons.Material.Outlined.LocationCity,
            fields: fields);

        if (values != null && values.TryGetValue("name", out var val) && val is string cityName && !string.IsNullOrWhiteSpace(cityName))
        {
            var (success, error, item) = await DictionaryService.AddCityAsync(cityName.Trim());
            if (success && item != null)
            {
                _cities.Add(item);
                Snackbar.Add($"Город «{item.Name}» успешно добавлен", Severity.Success);
            }
            else
            {
                Snackbar.Add(error ?? "Не удалось добавить город", Severity.Error);
            }
        }
    }

    // Добавление должности через динамический диалог
    private async Task OpenAddPositionDialogAsync()
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
        {
            new()
            {
                Key = "name",
                Label = "Название должности",
                Placeholder = "Например: Project Manager, QA Engineer",
                HelperText = "Стандартная грейдовая или функциональная позиция",
                FieldType = DynamicFormDialog.DynamicFieldType.Text,
                IsRequired = true
            }
        };

        var values = await ShowDynamicDialogAsync(
            title: "Новая должность",
            subtitle: "Добавление специализации в справочник позиций",
            icon: Icons.Material.Outlined.WorkOutline,
            fields: fields);

        if (values != null && values.TryGetValue("name", out var val) && val is string posName && !string.IsNullOrWhiteSpace(posName))
        {
            var (success, error, item) = await DictionaryService.AddPositionAsync(posName.Trim());
            if (success && item != null)
            {
                _positions.Add(item);
                Snackbar.Add($"Должность «{item.Name}» успешно добавлена", Severity.Success);
            }
            else
            {
                Snackbar.Add(error ?? "Не удалось добавить должность", Severity.Error);
            }
        }
    }

    private async Task<Dictionary<string, object?>?> ShowDynamicDialogAsync(
        string title,
        string subtitle,
        string icon,
        List<DynamicFormDialog.FormFieldConfig> fields)
    {
        var parameters = new DialogParameters<DynamicFormDialog>
        {
            { x => x.Title, title },
            { x => x.Subtitle, subtitle },
            { x => x.Icon, icon },
            { x => x.Fields, fields },
            { x => x.SubmitText, "Создать" }
        };

        var options = new DialogOptions 
        { 
            CloseOnEscapeKey = true, 
            MaxWidth = MaxWidth.Small, 
            FullWidth = true 
        };

        var dialog = await DialogService.ShowAsync<DynamicFormDialog>(title, parameters, options);
        var result = await dialog.Result;

        if (result is { Canceled: false, Data: Dictionary<string, object?> resValues })
        {
            return resValues;
        }

        return null;
    }
    // Редактирование навыка
    private async Task OpenEditSkillDialogAsync(DictionaryDTO item)
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
    {
        new()
        {
            Key = "name",
            Label = "Название навыка",
            StringValue = item.Name,
            FieldType = DynamicFormDialog.DynamicFieldType.Text,
            IsRequired = true
        }
    };

        var values = await ShowDynamicDialogAsync("Редактирование навыка", "Изменение названия компетенции", Icons.Material.Outlined.Edit, fields);
        if (values != null && values.TryGetValue("name", out var val) && val is string newName && !string.IsNullOrWhiteSpace(newName))
        {
            var (success, error, updated) = await DictionaryService.UpdateSkillAsync(item.Id, newName.Trim());
            if (success && updated != null)
            {
                var idx = _skills.FindIndex(s => s.Id == item.Id);
                if (idx >= 0) _skills[idx] = updated;
                Snackbar.Add("Навык успешно обновлен", Severity.Success);
            }
            else
            {
                Snackbar.Add(error ?? "Ошибка обновления", Severity.Error);
            }
        }
    }

    // Удаление навыка
    private async Task DeleteSkillAsync(DictionaryDTO item)
    {
        if (await DictionaryService.DeleteSkillAsync(item.Id))
        {
            _skills.RemoveAll(s => s.Id == item.Id);
            Snackbar.Add($"Навык «{item.Name}» удален", Severity.Info);
        }
        else
        {
            Snackbar.Add("Не удалось удалить навык (возможно, он привязан к вакансиям/кандидатам)", Severity.Warning);
        }
    }

    // Редактирование города
    private async Task OpenEditCityDialogAsync(DictionaryDTO item)
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
    {
        new() { Key = "name", Label = "Город", StringValue = item.Name, FieldType = DynamicFormDialog.DynamicFieldType.Text, IsRequired = true }
    };
        var values = await ShowDynamicDialogAsync("Редактирование локации", "Изменение города", Icons.Material.Outlined.Edit, fields);
        if (values != null && values.TryGetValue("name", out var val) && val is string newName && !string.IsNullOrWhiteSpace(newName))
        {
            var (success, error, updated) = await DictionaryService.UpdateCityAsync(item.Id, newName.Trim());
            if (success && updated != null)
            {
                var idx = _cities.FindIndex(c => c.Id == item.Id);
                if (idx >= 0) _cities[idx] = updated;
                Snackbar.Add("Город обновлен", Severity.Success);
            }
        }
    }

    private async Task DeleteCityAsync(DictionaryDTO item)
    {
        if (await DictionaryService.DeleteCityAsync(item.Id))
        {
            _cities.RemoveAll(c => c.Id == item.Id);
            Snackbar.Add($"Город «{item.Name}» удален", Severity.Info);
        }
        else Snackbar.Add("Не удалось удалить город", Severity.Warning);
    }

    // Редактирование должности
    private async Task OpenEditPositionDialogAsync(DictionaryDTO item)
    {
        var fields = new List<DynamicFormDialog.FormFieldConfig>
    {
        new() { Key = "name", Label = "Должность", StringValue = item.Name, FieldType = DynamicFormDialog.DynamicFieldType.Text, IsRequired = true }
    };
        var values = await ShowDynamicDialogAsync("Редактирование должности", "Изменение названия", Icons.Material.Outlined.Edit, fields);
        if (values != null && values.TryGetValue("name", out var val) && val is string newName && !string.IsNullOrWhiteSpace(newName))
        {
            var (success, error, updated) = await DictionaryService.UpdatePositionAsync(item.Id, newName.Trim());
            if (success && updated != null)
            {
                var idx = _positions.FindIndex(p => p.Id == item.Id);
                if (idx >= 0) _positions[idx] = updated;
                Snackbar.Add("Должность обновлена", Severity.Success);
            }
        }
    }

    private async Task DeletePositionAsync(DictionaryDTO item)
    {
        if (await DictionaryService.DeletePositionAsync(item.Id))
        {
            _positions.RemoveAll(p => p.Id == item.Id);
            Snackbar.Add($"Должность «{item.Name}» удалена", Severity.Info);
        }
        else Snackbar.Add("Не удалось удалить должность", Severity.Warning);
    }
    private async Task ToggleUserRoleAsync(UserProfileDTO user)
    {
        int newRoleId = user.RoleName == "Admin" ? (int)RoleEnum.Hr : (int)RoleEnum.Admin;
        var success = await AuthService.ChangeUserRoleAsync(user.Id, newRoleId);
        if (success)
        {
            Snackbar.Add($"Роль для {user.UserName} изменена", Severity.Success);
            await LoadDataAsync();
        }
    }
}
