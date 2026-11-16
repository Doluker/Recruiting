using Microsoft.AspNetCore.Components;
using MudBlazor;
using Recruiting.Client.Interfaces;
using Recruiting.Shared.DTOs;
using Recruiting.Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Recruiting.Client.Components.Pages;

public partial class NewVacancy
{
    [Inject] private IVacancyServiceClient VacancyService { get; set; } = default!;
    [Inject] private IDictionaryServiceClient DictionaryService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private bool _isLoading = true;
    private bool _isSaving = false;

    private ReferenceDataDTO? _references;

    private VacancyFormModel _model = new();

    // Добавление навыка
    private int? _selectedSkillToAdd;
    private bool _isSkillRequired = false;
    private record SelectedSkillItem(int SkillId, string SkillName, bool IsRequired);
    private List<SelectedSkillItem> _selectedSkills = new();

    public IReadOnlyList<DictionaryDTO> Cities => _references?.Cities ?? (IReadOnlyList<DictionaryDTO>)Array.Empty<DictionaryDTO>();
    public IReadOnlyList<DictionaryDTO> Educations => _references?.Educations ?? (IReadOnlyList<DictionaryDTO>)Array.Empty<DictionaryDTO>();
    public IEnumerable<DictionaryDTO> AvailableSkills =>
        (_references?.Skills ?? new List<DictionaryDTO>())
        .Where(s => !_selectedSkills.Any(sel => sel.SkillId == s.Id));

    public class VacancyFormModel
    {
        [Required(ErrorMessage = "Укажите название вакансии")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Длина названия от 3 до 200 символов")]
        public string Title { get; set; } = "";

        public string? Description { get; set; }

        public int? CityId { get; set; }
        public int? EducationId { get; set; }
        public int? SalaryFrom { get; set; }
        public int? SalaryTo { get; set; }
        public int? ExperienceYears { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _references = await DictionaryService.GetBootstrapAsync();
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка загрузки справочников: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void AddSkill()
    {
        if (!_selectedSkillToAdd.HasValue) return;

        var skill = _references?.Skills?.FirstOrDefault(s => s.Id == _selectedSkillToAdd.Value);
        if (skill != null && !_selectedSkills.Any(s => s.SkillId == skill.Id))
        {
            _selectedSkills.Add(new SelectedSkillItem(skill.Id, skill.Name, _isSkillRequired));
            _selectedSkillToAdd = null;
            _isSkillRequired = false;
        }
    }

    private void RemoveSkill(int skillId)
    {
        _selectedSkills.RemoveAll(s => s.SkillId == skillId);
    }

    private async Task HandleSubmit()
    {
        // По умолчанию при нажатии Submit публикуем активную вакансию
        await SaveVacancyAsync(isDraft: false);
    }

    private async Task SubmitWithStatus(bool isDraft)
    {
        await SaveVacancyAsync(isDraft);
    }

    private async Task SaveVacancyAsync(bool isDraft)
    {
        if (string.IsNullOrWhiteSpace(_model.Title))
        {
            Snackbar.Add("Введите название вакансии", Severity.Warning);
            return;
        }

        if (_model.SalaryFrom.HasValue && _model.SalaryTo.HasValue && _model.SalaryFrom > _model.SalaryTo)
        {
            Snackbar.Add("Зарплата «От» не может превышать «До»", Severity.Warning);
            return;
        }

        _isSaving = true;
        try
        {
            // Статус вакансии: если черновик — Draft, иначе Active (из справочника VacancyStatuses или enum)
            var activeStatus = _references?.VacancyStatuses?.FirstOrDefault(s => s.Name.Contains("Активн", StringComparison.OrdinalIgnoreCase))?.Id
                               ?? (int)VacancyStatusEnum.Active;
            var draftStatus = _references?.VacancyStatuses?.FirstOrDefault(s => s.Name.Contains("Чернов", StringComparison.OrdinalIgnoreCase))?.Id
                              ?? (int)VacancyStatusEnum.Draft;

            var statusId = isDraft ? draftStatus : activeStatus;

            // Источник: Manual
            var sourceId = _references?.Sources?.FirstOrDefault(s => s.Name.Contains("Ручн", StringComparison.OrdinalIgnoreCase))?.Id
                           ?? (int)SourceEnum.Manual;

            var skillsRequest = _selectedSkills
                .Select(s => new VacancySkillRequest(s.SkillId, s.IsRequired))
                .ToList();

            var request = new CreateVacancyRequest(
                Title: _model.Title,
                Description: _model.Description,
                VacancyStatusId: statusId,
                CityId: _model.CityId,
                SourceId: sourceId,
                SalaryFrom: _model.SalaryFrom,
                SalaryTo: _model.SalaryTo,
                ExperienceYears: _model.ExperienceYears,
                EducationId: _model.EducationId,
                Skills: skillsRequest);

            var (success, error, createdId) = await VacancyService.CreateVacancyAsync(request);

            if (success && createdId.HasValue)
            {
                Snackbar.Add("Вакансия успешно создана!", Severity.Success);
                Navigation.NavigateTo($"/workspace?vacId={createdId.Value}");
            }
            else
            {
                Snackbar.Add(error ?? "Не удалось сохранить вакансию", Severity.Error);
            }
        }
        catch (Exception ex)
        {
            Snackbar.Add($"Ошибка при сохранении: {ex.Message}", Severity.Error);
        }
        finally
        {
            _isSaving = false;
        }
    }
}