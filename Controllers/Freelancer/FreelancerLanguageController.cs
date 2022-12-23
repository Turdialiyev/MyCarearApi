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
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _languageService.GetAll(userId!);

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
    public async Task<IActionResult> Save([FromForm] FreelancerLanguage language)
    {
        try
        {
            var validate = new LanguageDtoValidation().Validate(language);
            
            if (!validate.IsValid)
                return BadRequest(validate.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _languageService.Save(userId!, ToModel(language));

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
            if (id < 0)
                return BadRequest(error:"Id invalid");

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