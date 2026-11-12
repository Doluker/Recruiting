using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VacanciesController : ControllerBase
{
    private readonly IVacancyService _vacancyService;

    public VacanciesController(IVacancyService vacancyService)
    {
        _vacancyService = vacancyService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] VacancyFilter filter)
    {
        var result = await _vacancyService.GetVacanciesAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var vacancy = await _vacancyService.GetVacancyByIdAsync(id);
        if (vacancy == null)
        {
            return NotFound(new ApiError("NOT_FOUND", "Вакансия не найдена или нет прав доступа"));
        }

        return Ok(vacancy);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVacancyRequest request)
    {
        var (success, error, id) = await _vacancyService.CreateVacancyAsync(request);
        if (!success)
        {
            return BadRequest(new ApiError("CREATE_FAILED", error ?? "Ошибка создания вакансии"));
        }

        return CreatedAtAction(nameof(GetById), new { id = id!.Value }, new OperationResult(true, "Вакансия создана", id));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateVacancyRequest request)
    {
        var (success, error) = await _vacancyService.UpdateVacancyAsync(id, request);
        if (!success)
        {
            return BadRequest(new ApiError("UPDATE_FAILED", error ?? "Ошибка обновления вакансии"));
        }

        return Ok(new OperationResult(true, "Вакансия успешно обновлена", id));
    }

    [HttpPatch("{id:int}/status/{statusId:int}")]
    public async Task<IActionResult> ChangeStatus(int id, int statusId)
    {
        var (success, error) = await _vacancyService.ChangeStatusAsync(id, statusId);
        if (!success)
        {
            return BadRequest(new ApiError("CHANGE_STATUS_FAILED", error ?? "Ошибка смены статуса"));
        }

        return Ok(new OperationResult(true, "Статус вакансии изменен", id));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _vacancyService.DeleteAsync(id);
        if (!success)
        {
            return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления вакансии"));
        }

        return Ok(new OperationResult(true, "Вакансия успешно удалена", id));
    }
}