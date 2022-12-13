using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FreelancerLanguageController : ControllerBase
{
    private readonly ILogger<FreelancerLanguageController> _logger;
    private readonly ILanguageService _languageService;

    public FreelancerLanguageController(ILogger<FreelancerLanguageController> logger, ILanguageService languageService)
    {
        _logger = logger;
        _languageService = languageService;
    }

    [HttpGet]
    [Produces("application/json")]
    public async Task<IActionResult> GatAll()
    {
        try
        {
            var result = await _languageService.GetAll();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPost("{id}")]
    public async Task<IActionResult> Save(int freelancerId, [FromForm] FreelancerLanguage language)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _languageService.Save(freelancerId, ToModel(language));

            if (!result.IsSuccess)
                return NotFound(result);

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
            var result = await _languageService.Delete(id);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
    private UserLanguage ToModel(FreelancerLanguage language) => new()
    {
        LanguageId = language.LanguageId,
        Level = language.Level
    };
}