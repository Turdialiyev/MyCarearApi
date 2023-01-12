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
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _educationService.GetAll(userId!);

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
    public async Task<IActionResult> Save([FromForm] FreelancerEducation education)
    {
        try
        {
            var validator = new EducationDtoValidation().Validate(education);

            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _educationService.Save(userId!, ToModel(education));

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
    public async Task<IActionResult> Update([FromRoute] int id, [FromForm] FreelancerEducation education)
    {
        try
        {
            var validator = new EducationDtoValidation().Validate(education);

            if (!validator.IsValid)
                return BadRequest(validator.Errors);

            if (id < 0)
                return BadRequest();

            var result = await _educationService.Update(id, ToModel(education));

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