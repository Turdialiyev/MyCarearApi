using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    private readonly ILogger<ProjectController> _logger;
    private readonly IProjectService _projectService;

    public ProjectController(
        ILogger<ProjectController> logger,
        IProjectService projectService)
    {
        _logger = logger;
        _projectService = projectService;
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromForm] FreelancerProject project)
    {
        var a = project.ProjectImages == null ? 0 : project.ProjectImages.Count();
        _logger.LogInformation($"=================>");

        _logger.LogInformation($"=================> {a}");
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _projectService.SaveAsync(userId!, project.Project!, project.ProjectImages, ToModel(project));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    private Models.FreelancerProject ToModel(FreelancerProject project) => new()
    {
      Title = project.Title,
      Description = project.Description,
      Tools = project.Tools,
      Link = project.Link,
    };
}