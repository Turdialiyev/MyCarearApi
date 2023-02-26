# pragma warning disable
using System.Security.Claims;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;
using MyCarearApi.Services;
using MyCarearApi.Validations;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public partial class FreelancerController : ControllerBase
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

    [HttpGet("Page/{page}/{size}")]
    public IActionResult GetByPage(int page, int size) => Ok(_freelancerService.GetByPage(page, size));

    [HttpGet("count")]
    public IActionResult GetCount() => Ok(new { Count = _freelancerService.GetCount() });

    [HttpGet("Freenalcer/Get")]
    public async Task<IActionResult> GetById()
    {
        try
        {
            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.GetById(userId);

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
        try
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            // var validator = new FreelancerDtoValidation().Validate(freelancer);

            // if (!validator.IsValid)
            //     return BadRequest(validator.Errors);

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

    [HttpPost("Adress")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Freelancer))]

    public async Task<IActionResult> FreelancerAdress([FromForm] Adress address)
    {

        try
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            var valid = new AdressDtoValidation().Validate(address);
    
            if (!valid.IsValid)
                return BadRequest(valid.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var result = await _freelancerService.Address(userId!, ToModelAddress(address));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }


    [HttpPost("Position")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Dtos.Position))]

    public async Task<IActionResult> FreelancerPosition([FromBody] Dtos.Position position)
    {
        try
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            // var validator = new PositionDtoValidation().Validate(position);

            // if (!validator.IsValid)
            //     return BadRequest(validator.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.Position(userId!, ToModelPosition(position));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Contact")]
    public async Task<IActionResult> FreelancerContact([FromForm] Dtos.FreelancerContact contact)
    {
        try
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            // var validator = new ContactDtoValidation().Validate(contact);

            // if (!validator.IsValid)
            //     return BadRequest(validator.Errors);

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.Contact(userId!, ToModelContact(contact));

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Resume")]
    public async Task<IActionResult> FreelancerResume([FromForm] int resume)
    {
        try
        {
            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            if (!ModelState.IsValid)
                return BadRequest();

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.Resume(userId!, resume);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);

        }
        catch (Exception e)
        {

            return StatusCode(StatusCodes.Status500InternalServerError, new { ErrorMessage = e.Message });
        }
    }

    [HttpPut("Finish")]
    public async Task<IActionResult> FreelancerFinish([FromForm] bool finish)
    {
        try
        {

            Console.WriteLine(HttpContext.Request.Headers.Authorization.ToString());
            if (!ModelState.IsValid)
                return BadRequest();

            string? userId = User.FindFirst(ClaimTypes.NameIdentifier) == null ? null : User.FindFirst(ClaimTypes.NameIdentifier)!.Value;


            var result = await _freelancerService.FinishResume(userId!, finish);

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
        NewHobbies= position.NewHobbies,
        NewSkills = position.NewSkills,
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
        Birthday = freelancer.BirthDay is null? null : DateOnly.FromDateTime(freelancer.BirthDay.Value)
    };
}