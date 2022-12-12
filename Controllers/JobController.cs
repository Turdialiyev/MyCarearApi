using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;
using MyCarearApi.Models.JobModels;
using MyCarearApi.Services;
using MyCarearApi.Services.JobServices;
using MyCarearApi.Services.JobServices.Interfaces;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace MyCarearApi.Controllers;

[Route("api/[controller]")]
public class JobController: ControllerBase
{
    private readonly IJobService _jobService;
    private readonly IJobLanguagesService _jobLanguagesService;
    private readonly IJobSkillsService _jobSkillsService;
    private readonly UserManager<Entities.AppUser> _userManager;

    public JobController(IJobService jobService, UserManager<Entities.AppUser> userManager, 
        IJobLanguagesService jobLanguagesService, IJobSkillsService jobSkillsService)
    {
        _jobService = jobService;
        _jobLanguagesService = jobLanguagesService;
        _jobSkillsService = jobSkillsService;
        _userManager= userManager;
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
    public IActionResult StartCreateJob(JobTitle job)
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
            JobId = _jobService.AddJob(job.Title, job.PositionId, company.Id)
        });
    }

    [HttpPost("description")]
    [Authorize]
    public async Task<IActionResult> DescribeJob([FromForm]int jobId, [FromForm] string description, [FromForm] IFormFile? file)
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
            Id = jobId
        });
    }

    [HttpPost("talant")]
    [Authorize]  
    public IActionResult SetTalantRequirements(TalantRequirementsModel talant) 
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(talant.JobId);

        var notFoundSkills = _jobSkillsService.CheckSkillIds(talant.requiredSkillIds);

        var notFoundLanguages = _jobLanguagesService.CheckLanguageIds(talant.requiredLanguageIds);
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
            Id = _jobService.SetTalantRequirements(talant.JobId, talant.reuiredCandidateLevel,
                                                   talant.requiredSkillIds, talant.requiredLanguageIds)
        });
    }

    [HttpPost("contract")]
    [Authorize]
    public async Task<IActionResult> SetContractRequirements(ContractRequirementsModel contract)
    {
        var company = _jobService.GetCompany(User.FindFirst(ClaimTypes.NameIdentifier).Value);
        var job = _jobService.GetJob(contract.JobId);

        if(company is null || job is null || company.Id != job.CompanyId || _jobService.IsCurrencyExist(contract.CurrencyId)
            || contract.Deadline < 1)
        {
            return BadRequest(new
            {
                Succeded = false,
                OwnerError = company is null || job is null || company.Id != job.CompanyId,
                CurrencyError = _jobService.IsCurrencyExist(contract.CurrencyId),
                DeadlineError = contract.Deadline < 1
            });
        }

        return Ok(new
        {
            Succeded = true,
            Id = await _jobService.SetContractRequirements(contract.JobId, contract.Price, contract.CurrencyId,
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
}
