using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CandidatesController : ControllerBase
{
    private readonly ICandidateService _candidateService;

    public CandidatesController(ICandidateService candidateService)
    {
        _candidateService = candidateService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] CandidateFilter filter)
    {
        var result = await _candidateService.GetCandidatesAsync(filter);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var candidate = await _candidateService.GetCandidateByIdAsync(id);
        if (candidate == null)
            return NotFound(new ApiError("NOT_FOUND", "Кандидат не найден или нет прав доступа"));

        return Ok(candidate);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCandidateRequest request)
    {
        var (success, error, id) = await _candidateService.CreateCandidateAsync(request);
        if (!success)
            return BadRequest(new ApiError("CREATE_FAILED", error ?? "Ошибка создания кандидата"));

        return CreatedAtAction(nameof(GetById), new { id = id!.Value }, new OperationResult(true, "Кандидат успешно создан", id));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCandidateRequest request)
    {
        var (success, error) = await _candidateService.UpdateCandidateAsync(id, request);
        if (!success)
            return BadRequest(new ApiError("UPDATE_FAILED", error ?? "Ошибка обновления кандидата"));

        return Ok(new OperationResult(true, "Данные кандидата обновлены", id));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var (success, error) = await _candidateService.DeleteCandidateAsync(id);
        if (!success)
            return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления кандидата"));

        return Ok(new OperationResult(true, "Кандидат удален", id));
    }
}