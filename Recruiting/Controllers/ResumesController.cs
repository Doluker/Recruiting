using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Recruiting.Interfaces;
using Recruiting.Shared.DTOs;

namespace Recruiting.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResumesController : ControllerBase
{
    private readonly IResumeService _resumeService;

    public ResumesController(IResumeService resumeService)
    {
        _resumeService = resumeService;
    }

    [HttpPost("upload")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<UploadResumeResponse>> Upload(IFormFile file, CancellationToken ct)
    {
        var result = await _resumeService.UploadResumeAsync(file, ct);
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ResumeFileDTO>> GetById(int id, CancellationToken ct)
    {
        var file = await _resumeService.GetByIdAsync(id, ct);
        if (file == null) return NotFound();
        return Ok(file);
    }

    [HttpGet("{id:int}/download")]
    public async Task<IActionResult> Download(int id, CancellationToken ct)
    {
        var (stream, contentType, fileName) = await _resumeService.DownloadResumeFileAsync(id, ct);
        return File(stream, contentType, fileName);
    }
}