using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

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
            var result = await _experienceService.GetAll();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPost("{freelancerId}")]
    public async Task<IActionResult> Save(int freelancerId, [FromForm] FreelancerExperience experience)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _experienceService.Save(freelancerId, ToModel(experience));

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

            if (!ModelState.IsValid)
                return BadRequest();

            if (id < 0)
                return BadRequest();
            
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