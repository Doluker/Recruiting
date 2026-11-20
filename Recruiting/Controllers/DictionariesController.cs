using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DictionariesController : ControllerBase
{
    private readonly IDictionaryService _dictionaryService;

    public DictionariesController(IDictionaryService dictionaryService)
    {
        _dictionaryService = dictionaryService;
    }

    /// <summary>
    /// Главный метод: отдает все справочники сразу за один запрос.
    /// </summary>
    [HttpGet("bootstrap")]
    public async Task<IActionResult> GetBootstrap()
    {
        var data = await _dictionaryService.GetBootstrapDataAsync();
        return Ok(data);
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities([FromQuery] string? search)
    {
        var cities = await _dictionaryService.GetCitiesAsync(search);
        return Ok(cities);
    }

    [HttpPost("cities")]
    [Authorize]
    public async Task<IActionResult> AddCity([FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.AddCityAsync(request.Name);
        if (!success)
            return BadRequest(new ApiError("INVALID_CITY", error ?? "Ошибка добавления города"));

        return Ok(item);
    }

    [HttpGet("skills")]
    public async Task<IActionResult> GetSkills([FromQuery] string? search)
    {
        var skills = await _dictionaryService.GetSkillsAsync(search);
        return Ok(skills);
    }

    [HttpPost("skills")]
    [Authorize]
    public async Task<IActionResult> AddSkill([FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.AddSkillAsync(request.Name);
        if (!success)
            return BadRequest(new ApiError("INVALID_SKILL", error ?? "Ошибка добавления навыка"));

        return Ok(item);
    }

    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions([FromQuery] string? search)
    {
        var positions = await _dictionaryService.GetPositionsAsync(search);
        return Ok(positions);
    }

    [HttpPost("positions")]
    [Authorize]
    public async Task<IActionResult> AddPosition([FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.AddPositionAsync(request.Name);
        if (!success)
            return BadRequest(new ApiError("INVALID_POSITION", error ?? "Ошибка добавления должности"));

        return Ok(item);
    }
    [HttpPut("skills/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateSkill(int id, [FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.UpdateSkillAsync(id, request.Name);
        if (!success) return BadRequest(new ApiError("UPDATE_FAILED", error ?? "Ошибка обновления навыка"));
        return Ok(item);
    }

    [HttpDelete("skills/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSkill(int id)
    {
        var (success, error) = await _dictionaryService.DeleteSkillAsync(id);
        if (!success) return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления навыка"));
        return Ok(new OperationResult(true, "Навык удален"));
    }

    [HttpPut("cities/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateCity(int id, [FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.UpdateCityAsync(id, request.Name);
        if (!success) return BadRequest(new ApiError("UPDATE_FAILED", error ?? "Ошибка обновления города"));
        return Ok(item);
    }

    [HttpDelete("cities/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteCity(int id)
    {
        var (success, error) = await _dictionaryService.DeleteCityAsync(id);
        if (!success) return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления города"));
        return Ok(new OperationResult(true, "Город удален"));
    }

    [HttpPut("positions/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdatePosition(int id, [FromBody] CreateDictionaryItemRequest request)
    {
        var (success, error, item) = await _dictionaryService.UpdatePositionAsync(id, request.Name);
        if (!success) return BadRequest(new ApiError("UPDATE_FAILED", error ?? "Ошибка обновления должности"));
        return Ok(item);
    }

    [HttpDelete("positions/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        var (success, error) = await _dictionaryService.DeletePositionAsync(id);
        if (!success) return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления должности"));
        return Ok(new OperationResult(true, "Должность удалена"));
    }
}