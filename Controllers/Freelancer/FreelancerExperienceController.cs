# pragma warning disable
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;
using MyCarearApi.Validations;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FreelancerExperienceController : ControllerBase
{
    private readonly ILogger<FreelancerExperienceController> _logger;
    private readonly IExperienceService _experienceService;

    public FreelancerExperienceController(ILogger<FreelancerExperienceController> logger, IExperienceService experienceService)
    {
        _logger = logger;
        _experienceService = experienceService;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GatAll()
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _experienceService.GetAll(userId!);

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Save([FromForm] FreelancerExperience experience)
    {
        try
        {
            var validation = new ExperienceDtoValidation().Validate(experience);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _experienceService.Save(userId!, ToModel(experience));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] FreelancerExperience experience)
    {
        try
        {
            var validation = new ExperienceDtoValidation().Validate(experience);

            if (!validation.IsValid)
                return BadRequest(validation.Errors);


            if (id < 0)
                return BadRequest(error: "id invalid");

            var result = await _experienceService.Update(id, ToModel(experience));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            if (id < 0)
                return BadRequest(error: "id invalid");

            var result = await _experienceService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    private Experience ToModel(FreelancerExperience experience) => new()
    {
        CompanyName = experience.CompanyName,
        Job = experience.Job,
        Descripeion = experience.Descripeion,
        CurrentWorking = experience.CurrentWorking
    };

}