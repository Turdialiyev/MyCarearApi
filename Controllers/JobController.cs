# pragma warning disable
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;
using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;
using MyCarearApi.Models.JobModels;
using MyCarearApi.Services;
using MyCarearApi.Services.JobServices;
using MyCarearApi.Services.JobServices.Interfaces;
using System.ComponentModel;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MyCarearApi.Controllers;

[Route("api/[controller]")]
public class JobController: ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IJobLanguagesService _jobLanguagesService;
    private readonly IJobSkillsService _jobSkillsService;
    private readonly IOfferService _offerService;
    public readonly IFreelancerService _freelancerService;
    private readonly UserManager<Entities.AppUser> _userManager;

    public JobController(IJobService jobService, UserManager<Entities.AppUser> userManager, 
        IJobLanguagesService jobLanguagesService, IJobSkillsService jobSkillsService, 
        IOfferService offerService, IFreelancerService freelancerService)
    {
        _jobService = jobService;
        _jobLanguagesService = jobLanguagesService;
        _jobSkillsService = jobSkillsService;
        _userManager = userManager;
        _offerService = offerService;
        _freelancerService = freelancerService;
    }

    private Regex spaceReplacer = new Regex(@"\s\s+");

    private string SpaceReplace(string text)
    {
        text = spaceReplacer.Replace(text, " ");
        if (text == " ") return string.Empty;
        if (text[0] == ' ')
        {
            return text.Substring(1);
        }
        return text;
    }

    [HttpPost("title")]
    [Authorize]
    public IActionResult StartCreateJob([FromBody]JobTitle job)
    {
        job.Title = SpaceReplace(job.Title);

        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        if(string.IsNullOrWhiteSpace(job.Title) || !_jobService.IsPositionExist(job.PositionId) || company is null)
        {
            return BadRequest(new
            {
                Succeded = false,
                TitleError = string.IsNullOrWhiteSpace(job.Title),
                CategoryError = _jobService.IsPositionExist(job.PositionId),
                CompanyNotFount = company is null
            });
        }

        return Ok(new
        {
            Succeded = true,
            JobId = _jobService.AddJob(job.JobId, job.Title, job.PositionId, company.Id)
        });
    }

    [HttpPost("description")]
    [Authorize]
    public async Task<IActionResult> DescribeJob(int jobId, string description, IFormFile? file)
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(jobId);


        if (company is null || job.CompanyId !=company.Id || string.IsNullOrWhiteSpace(SpaceReplace(description)) 
            || string.IsNullOrEmpty(description))
        {
            return BadRequest(new
            {
                Succeded = false,
                DescriptionError = string.IsNullOrWhiteSpace(SpaceReplace(description)) || string.IsNullOrEmpty(description),
                CompanyNotFound = company is null || job.CompanyId != company.Id
            });
        }

        jobId = await _jobService.AddDescriptionToJob(company.Id, description, file);

        return Ok(new
        {
            Succeded = true,
            JobId = jobId
        });
    }
    
    [HttpPost("talant")]
    [Authorize]
    public IActionResult SetTalantRequirements([FromBody]TalantRequirementsModel talant) 
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(talant.JobId);

        var notFoundSkills = _jobSkillsService.CheckSkillIds(talant.RequiredSkillIds);

        var notFoundLanguages = _jobLanguagesService.CheckLanguageIds(talant.RequiredLanguageIds);
        //Validation
        if (company is null || job is null || company.Id != job.CompanyId
            || notFoundSkills.Count() > 0 || notFoundLanguages.Count() > 0)
            return BadRequest(new
            {
                Succeded = false,
                OwnerError = company is null || job is null || company.Id != job.CompanyId,
                SkillError = notFoundSkills.Count() > 0,
                NotFoundSkills = notFoundSkills,
                LanguageError = notFoundLanguages.Count() > 0,
                NotFoundLanguages = notFoundLanguages
            });


        return Ok(new
        {
            Succeded = true,
            JobId = _jobService.SetTalantRequirements(talant.JobId, talant.RequiredCandidateLevel,
                                                   talant.RequiredSkillIds, talant.RequiredLanguageIds)
        });
    }

    [HttpPost("contract")]
    [Authorize]
    public async Task<IActionResult> SetContractRequirements([FromBody]ContractRequirementsModel contract)
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(contract.JobId);

        if(company is null || job is null || company.Id != job.CompanyId || !_jobService.IsCurrencyExist(contract.CurrencyId)
            || contract.Deadline < 1)
        {
            return BadRequest(new
            {
                Succeded = false,
                OwnerError = company is null || job is null || company.Id != job.CompanyId,
                CurrencyError = !_jobService.IsCurrencyExist(contract.CurrencyId),
                DeadlineError = contract.Deadline < 1
            });
        }

        return Ok(new
        {
            Succeded = true,
            JobId = await _jobService.SetContractRequirements(contract.JobId, contract.Price, contract.CurrencyId,
            contract.PriceRate, contract.Deadline, contract.DeadlineRate)
        });
    }

    [HttpPost("save")]
    [Authorize]
    public async Task<IActionResult> SaveJob(int jobId)
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(jobId);

        if (company is null || job is null || company.Id != job.CompanyId)
        {
            return BadRequest(new
            {
                Succeded = false,
                OwnerError = company is null || job is null || company.Id != job.CompanyId
            });
        }
        job.IsSaved = true;        
        return Ok(new
        {
            Succeded = true,
            JobId = await _jobService.UpdateJob(job)
        });
    }

    [HttpPost("offer")]
    [Authorize]
    public IActionResult Offer([FromBody] OfferCreateModel offer)
    {
        var job = _jobService.GetJob(offer.JobId);
        var company = _jobService.GetCompany(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (job is null || job.State != JobState.Active || company is null || company.Id != job.CompanyId)
            return BadRequest(new
            {
                Succeded = false
            });

        return Ok(new
        {
            Succeded = true,
            Offer = _offerService
            .AddOffer(offer.JobId, offer.Downpayment, offer.Deadline, offer.DeadlineRate, offer.FreelancerId)
        });
    }

    [HttpGet("offer/{offerId}")]
    [Authorize]
    public IActionResult Offer(int offerId) => _offerService.GetOffer(offerId) is not null
                    ? Ok(new {Succeded = true, Offer = _offerService.GetOffer(offerId) })
                    : BadRequest(new { Succeded = false });

    [HttpGet("companyOffers")]
    [Authorize]
    public IActionResult GetCompanyOffers()
    {
        var company = _jobService.GetCompany(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if(company is null)
            return BadRequest(new { Succeded = false });
        return Ok(new
        {
            Succeded = true,
            Offers = _offerService.GetCompanyOffers(company.Id)
        });
    }

    [HttpGet("freelancerOffers")]
    [Authorize]
    public IActionResult GetFreelancerOffers()
    {
        var offers = _offerService.GetFreelancerOffers(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if(offers is null || offers.Count == 0)
        {
            return Ok(new
            {
                Succeded = true,
                Offers = new List<Offer>()
            });
        }
        return Ok(new
        {
            Succeded = true,
            Offers = offers
        });
    }

    [HttpGet("All")]
    [Authorize]
    public IActionResult GetAllJobs()
    {
        var company = _jobService.GetCompany(User.FindFirstValue(ClaimTypes.NameIdentifier));
        if (company is null) return BadRequest(new
        {
            Succeded = false,
            Error = "You need to create a company"
        });
        return Ok(new
        {
            Succeded = true,
            Jobs = _jobService.GetJobsOfComapany(company.Id).Select(x => JobDto(x))
        });
    }

    [HttpGet("Pag/{page}/{size}")]
    public IActionResult GetByPage(int page, int size) => Ok(new
    {
        Succeded = true,
        Jobs = _jobService.GetByPage(page, size).Select(x => JobDto(x))
    });

    [HttpGet("/{jobId}")]
    [Authorize]
    public IActionResult GetById(int jobId)
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(jobId);

        if (company is null || job is null || company.Id != job.CompanyId)
        {
            return BadRequest(new
            {
                Succeded = false,
                OwnerError = company is null || job is null || company.Id != job.CompanyId
            });
        }

        return Ok(JobDto(job));
    }

    [HttpGet("count")]
    public IActionResult GetCount() => Ok(new { Count = _jobService.GetCount() });

    private dynamic JobDto(Entities.Job job) => new
    {
        Succeded = true,
        Job = new
        {
            job.Id,
            job.FilePath,
            JobSkills = job.JobSkills.Select(x => new
            {
                x.Id,
                x.JobId,
                x.SkillId,
                Skill = new
                {
                    x.Skill.Name,
                    x.Skill.Id,
                    x.Skill.PositionId
                }
            }),
            JobLanguages = job.JobLanguages.Select(x => new
            {
                x.Id,
                x.JobId,
                x.LanguageId,
                x.Language
            }),
            job.State,
            job.CompanyId,
            Company = new
            {
                job.Company.Id,
                job.Company.Name,
                job.Company.PhoneNumber,
                job.Company.Photo,
                job.Company.CompanyLocations,
                job.Company.AppUserId,
                Email = job.Company.AppUser.CopmanyEmail,
                AppUser = new
                {
                    job.Company.AppUser.Id,
                    job.Company.AppUser.FirstName,
                    job.Company.AppUser.LastName,
                    job.Company.AppUser.Email,
                    job.Company.AppUser.PhoneNumber
                }
            },
            job.CurrencyId,
            job.Currency,
            job.DeadLine,
            job.DeadlineRate,
            job.Description,
            job.IsSaved,
            job.PositionsId,
            job.Position,
            job.Price,
            job.PriceRate,
            job.RequiredLevel
        }
    };
}
