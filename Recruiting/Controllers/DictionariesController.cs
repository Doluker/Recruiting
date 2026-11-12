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
}