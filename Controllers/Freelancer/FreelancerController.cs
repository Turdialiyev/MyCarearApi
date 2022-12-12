using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public class FreelancerController : ControllerBase
{
    private readonly ILogger<FreelancerController> _logger;
    private readonly IFileHelper _fileHelper;
    private readonly IFreelancerService _freelancerService;

    public FreelancerController(
        ILogger<FreelancerController> logger,
        IFileHelper fileHepler,
        IFreelancerService freelancerService)
    {
        _logger = logger;
        _fileHelper = fileHepler;
        _freelancerService = freelancerService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Freelancer))]
    public async Task<IActionResult> FreelancerInformation([FromForm] Freelancer freelancer)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.Information("1231231", ToModel(freelancer), freelancer.Image!);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }

    }

    [HttpPost("Adress/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Freelancer))]

    public async Task<IActionResult> FreelancerAdress(int freelancerId, [FromBody] Adress address)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.Address(freelancerId, ToModelAddress(address));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }


    [HttpPost("Position/{id}")]
    public async Task<IActionResult> FreelancerPosition(int freelancerId, [FromBody] Dtos.Position position)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.Position(freelancerId, ToModelPosition(position));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    private Models.Position ToModelPosition(Dtos.Position position) => new()
    {
        Description = position.Description,
        PositionId = position.PositionId,
        PositionSkills = ToModelSkills(position.FreelancerSkills),
        FreelancerHobbies = TOModelHobbies(position.FreelancerHobbies),
    };

    private IEnumerable<Models.FreelancerHobby> TOModelHobbies(int[]? freelancerHobbies)
    {
        return freelancerHobbies!.Select(x => new Models.FreelancerHobby
        {
            HobbyId = x
        });
    }

    private IEnumerable<Models.FreelancerSkill> ToModelSkills(int[]? freelancerSkills)
    {
        return freelancerSkills!.Select(x => new Models.FreelancerSkill
        {
            SkillId = x
        });
    }

    private Address ToModelAddress(Adress address) => new()
    {
        CountryId = address.CountryId,
        RegionId = address.RegionId,
        Home = address.Home,
    };

    private FreelancerInformation ToModel(Freelancer freelancer) => new()
    {
        FirstName = freelancer.FirstName,
        LastName = freelancer.LastName,
        Email = freelancer.Email,
        PhoneNumber = freelancer.Phone,
    };
}