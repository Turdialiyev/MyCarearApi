using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;
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
    private readonly UserManager<Entities.AppUser> _userManager;

    public JobController(IJobService jobService, UserManager<Entities.AppUser> userManager)
    {
        _jobService = jobService;
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
            return Ok(new
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
            return Ok(new
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
    public IActionResult SettalantRequirements(int jobId, Level level, IEnumerable<int> skills, IEnumerable<int> languages) 
    {
        return Ok();
    }

}
