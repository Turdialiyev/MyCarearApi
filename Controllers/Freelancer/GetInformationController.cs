using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class GetInformationController : ControllerBase
{
    private readonly IGetInformationService _getInformationService;

    public GetInformationController(IGetInformationService getInformationService)
    {
        _getInformationService = getInformationService;
    }

    [HttpGet("Countries")]
    public async Task<IActionResult> GetCountry()
    {
        try
        {
            var result = await _getInformationService.GetCountries();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpGet("Hobbies")]
    public async Task<IActionResult> GetHobby()
    {
        try
        {
            var result = await _getInformationService.GetHobbies();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpGet("Positions")]
    public async Task<IActionResult> GetPosition()
    {
        try
        {
            var result = await _getInformationService.GetPositions();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
    [HttpGet("Languages")]
    public async Task<IActionResult> GetLanguage()
    {
        try
        {
            var result = await _getInformationService.GetLanguages();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
}