using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Entities;
using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;
using MyCarearApi.Services.JobServices;
using System.Text.RegularExpressions;

namespace MyCarearApi.Controllers;

public class JobController: ControllerBase
{
    private readonly JobService _jobService;
    

    public JobController(JobService jobService)
    {
        _jobService = jobService;
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
    public IActionResult StartCreateJob(JobTitle job)
    {
        job.Title = SpaceReplace(job.Title);
        if(string.IsNullOrWhiteSpace(job.Title) || !_jobService.IsPositionExist(job.PositionId))
        {
            return Ok(new
            {
                Succeded = false,
                TitleError = true,
                CategoryError = _jobService.IsPositionExist(job.PositionId)
            });
        }
        return Ok(new
        {
            Succeded = true,
            JobId = _jobService.AddJob(job.Title, job.PositionId)
        });
    }


    
}
