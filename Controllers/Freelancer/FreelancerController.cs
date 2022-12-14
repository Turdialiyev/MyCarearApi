using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Entities.Enums;
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Models.FreelancerInformation))]

    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _freelancerService.GetAll();

            if (!result.IsSuccess)
                return NotFound(new { ErrorMessage = result.ErrorMessage });

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int freelancerId)
    {
        try
        {
            var result = await _freelancerService.GetById(freelancerId);

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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Freelancer))]
    public async Task<IActionResult> FreelancerInformation([FromForm] Freelancer freelancer)
    {
        _logger.LogInformation("================>>>>>>>>>>>>>>");
        try
        {
            if (!ModelState.IsValid)
                return BadRequest();

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.Information(userId!, ToModel(freelancer), freelancer.Image!);

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

    public async Task<IActionResult> FreelancerAdress(int freelancerId, [FromForm] Adress address)
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
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dtos.Position))]

    public async Task<IActionResult> FreelancerPosition(int freelancerId, [FromForm] Dtos.Position position)
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

    [HttpPut("Contact/{id}")]
    public async Task<IActionResult> FreelancerContact(int freelancerId, [FromForm] Dtos.FreelancerContact contact)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.Contact(freelancerId, ToModelContact(contact));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Resume/{id}")]
    public async Task<IActionResult> FreelancerResume(int freelancerId, TypeResume resume)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.Resume(freelancerId, resume);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Finish/{id}")]
    public async Task<IActionResult> FreelancerFinish(int freelancerId, bool finish)
    {
        try
        {

            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _freelancerService.FinishResume(freelancerId, finish);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }
    private Models.FreelancerContact ToModelContact(Dtos.FreelancerContact contact) => new()
    {
        WebSite = contact.WebSite,
        WatsApp = contact.WatsApp,
        Telegram = contact.Telegram,
        Instagram = contact.Instagram,
        Twitter = contact.Twitter,
        GitHub = contact.GitHub,
        Facebook = contact.Facebook,
    };

    private Models.Position ToModelPosition(Dtos.Position position) => new()
    {
        Description = position.Description,
        PositionId = position.PositionId,
        PositionSkills = ToModelSkills(position.FreelancerSkills),
        FreelancerHobbies = ToModelHobbies(position.FreelancerHobbies),
    };

    private IEnumerable<Models.FreelancerHobby> ToModelHobbies(int[]? freelancerHobbies)
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