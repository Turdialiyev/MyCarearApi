using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FreelancerEducationController : ControllerBase
{
    private readonly ILogger<FreelancerEducationController> _logger;
    private readonly IEducationService _educationService;

    public FreelancerEducationController(ILogger<FreelancerEducationController> logger, IEducationService educationService)
    {
        _logger = logger;
        _educationService = educationService;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GatAll()
    {
        try
        {
            var result = await _educationService.GetAll();

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
    public async Task<IActionResult> Save(int freelancerId, [FromForm] FreelancerEducation rducation)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _educationService.Save(freelancerId, ToModel(rducation));

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
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] FreelancerEducation educarion)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest();

            if (id < 0)
                return BadRequest();

            var result = await _educationService.Update(id, ToModel(educarion));

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    private Education ToModel(FreelancerEducation educarion) => new()
    {
        SchoolName = educarion.SchoolName,
        EducationDegree = educarion.EducationDegree,
        TypeStudy = educarion.TypeStudy,
        Location = educarion.Location,
        CurrentStudy = educarion.CurrentStudy
    };

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result = await _educationService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }


}