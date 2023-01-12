# pragma warning disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class ExperienceService : IExperienceService
{
    private readonly ILogger<ExperienceService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.AppUser> _userManager;

    public ExperienceService(
        ILogger<ExperienceService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;

    }
    public async ValueTask<Result<Experience>> Save(string userId, Experience experience)
    {
        try
        {
            if (experience is null)
                return new(false) { ErrorMessage = "experience Null exception" };

            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "User Id not found" };

            var existInformation = _unitOfWork.FreelancerInformations.GetAll().FirstOrDefault(f => f.AppUserId == userId);

            if (existInformation is null)
                return new(false) { ErrorMessage = "Freelancer information not found" };

            var result = await _unitOfWork.Experiences.AddAsync(ToEntity(experience, existInformation.Id));

            return new(true) { Data = ToModel(result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ExperienceService)}", e);

            throw new("Couldn't create Experience. Plaese contact support", e);
        }


    }

    private Experience ToModel(Entities.Experience result) => new()
    {
        Id = result.Id,
        CompanyName = result.CompanyName,
        Job = result.Job,
        CurrentWorking = result.CurrentWorking,
        StartDate = result.StartDate,
        EndDate = result.EndDate,
        Descripeion = result.Descripeion,
        FrelancerInformationId = result.FreelancerInformationId
    };

    private Entities.Experience ToEntity(Experience model, int freelanserId) => new()
    {
        CompanyName = model.CompanyName,
        CurrentWorking = model.CurrentWorking,
        Job = model.Job,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        Descripeion = model.Descripeion,
        FreelancerInformationId = freelanserId,
    };

    public async ValueTask<Result<Experience>> Delete(int experienceId)
    {
        try
        {
            var existExperience = _unitOfWork.Experiences.GetById(experienceId);
            if (existExperience is null)
                return new("Experience with given Id Not Found");

            var result = await _unitOfWork.Experiences.Remove(existExperience);

            return new(true) { Data = ToModel(result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ExperienceService)}", e);
            throw new("Couldn't delete experience. Plaese contact support", e);

        }
    }

    public async ValueTask<Result<Experience>> Update(int experienceId, Experience experience)
    {
        try
        {

            if (experienceId == 0)
                return new("Given category Id invalid");

            if (experience is null)
                return new("Experience null");

            var existExperience = _unitOfWork.Experiences.GetById(experienceId);

            if (existExperience is null)
                return new("Experience given Id not found");

            existExperience.CompanyName = experience.CompanyName;
            existExperience.Job = experience.Job;
            existExperience.Descripeion = experience.Descripeion;
            existExperience.StartDate = experience.StartDate;
            existExperience.EndDate = experience.EndDate;
            existExperience.CurrentWorking = experience.CurrentWorking;

            var updated = await _unitOfWork.Experiences.Update(existExperience);

            return new(true) { Data = ToModel(updated) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ExperienceService)}");
            throw new("Couldn't update Experience. Please contact support", e);
        }
    }

    public async ValueTask<Result<List<Experience>>> GetAll(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "User Id not found" };

            var existInformation = _unitOfWork.FreelancerInformations.GetAll().FirstOrDefault(f => f.AppUserId == userId);

            if (existInformation is null)
                return new(false) { ErrorMessage = "Freelancer information not found" };

            var experiences = await _unitOfWork.Experiences.GetAll().Where(x => x.FreelancerInformationId == existInformation.Id).ToListAsync();

            if (!experiences.Any())
                return new(false) { ErrorMessage = "Any Experience not found" };

            return new(true) { Data = experiences.Select(ToModel).ToList() };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ExperienceRepository)} .");
            throw new("Couldn't get Experience Please contact support");
        }
    }
}