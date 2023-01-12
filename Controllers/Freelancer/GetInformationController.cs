# pragma warning disable
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities.Enums;
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
        Console.WriteLine("==============================> ");
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

    [HttpGet("Skills")]
    public async Task<IActionResult> GetSkills() => Ok( await _getInformationService.GetSkills());

    [HttpGet("CandidateLeves")]
    public IActionResult GetLevels() => Ok(new
    {
        Data = Enum.GetValues<Level>().ToDictionary(x => (int)x, x => x.ToString())
    });

    [HttpGet("PriceRates")]
    public IActionResult GetPriceRates() => Ok(new
    {
        Data = Enum.GetValues<PriceRate>().ToDictionary(x => (int)x, x => x.ToString())
    });

    [HttpGet("DeadlineRates")]
    public IActionResult GetDeadlineRates() => Ok(new
    {
        Data = Enum.GetValues<DeadlineRate>().ToDictionary(x => (int)x, x => x.ToString())
    });

    [HttpGet("Currencies")]
    public IActionResult GetCurrencies() => Ok(_getInformationService.GetCurrencies());

}