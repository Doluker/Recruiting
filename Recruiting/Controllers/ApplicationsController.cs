using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ApplicationsController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        public ApplicationsController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ApplicationFilter filter)
        {
            var result = await _applicationService.GetApplicationsAsync(filter);
            return Ok(result);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var app = await _applicationService.GetApplicationByIdAsync(id);
            if (app == null)
                return NotFound(new ApiError("NOT_FOUND", "Отклик не найден или нет прав доступа"));
            return Ok(app);
        }
        [HttpGet("{id:int}/history")]
        public async Task<IActionResult> GetHistory(int id)
        {
            var history = await _applicationService.GetHistoryAsync(id);
            return Ok(history);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateApplicationRequest request)
        {
            var (success, error, id) = await _applicationService.CreateApplicationAsync(request);
            if (!success)
                return BadRequest(new ApiError("CREATE_FAILED", error ?? "Ошибка создания отклика"));
            return CreatedAtAction(nameof(GetById), new { id = id!.Value }, new OperationResult(true, "Отклик успешно создан", id));
        }
        [HttpPost("{id:int}/stage")]
        public async Task<IActionResult> ChangeStage(int id, [FromBody] ChangeStageRequest request)
        {
            var (success, error) = await _applicationService.ChangeStageAsync(id, request);
            if (!success)
                return BadRequest(new ApiError("CHANGE_STAGE_FAILED", error ?? "Ошибка смены этапа"));
            return Ok(new OperationResult(true, "Этап отклика успешно изменён", id));
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var (success, error) = await _applicationService.DeleteApplicationAsync(id);
            if (!success)
                return BadRequest(new ApiError("DELETE_FAILED", error ?? "Ошибка удаления отклика"));
            return Ok(new OperationResult(true, "Отклик удалён", id));
        }
    }
}
