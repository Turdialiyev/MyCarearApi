using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class ExperienceService : IExperienceService
{
    private readonly ILogger<ExperienceService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public ExperienceService(ILogger<ExperienceService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;

    }
    public async ValueTask<Result<Experience>> Save(int freelncerId, Experience experience)
    {
        if (experience is null)
            return new("Experience null exseption");

        try
        {
            var exsitFreelancer = _unitOfWork.FreelancerInformations.GetById(freelncerId);

            if (exsitFreelancer is null)
                return new("exsitFreelancer is not found");

            var result = await _unitOfWork.Experiences.AddAsync(ToEntity(experience, freelncerId));

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
        FrelancerInformationId = result.FrelancerInfoId
    };

    private Entities.Experience ToEntity(Experience model, int freelanserId) => new()
    {
        CompanyName = model.CompanyName,
        CurrentWorking = model.CurrentWorking,
        Job = model.Job,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        Descripeion = model.Descripeion,
        FrelancerInfoId = freelanserId,
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

    public async ValueTask<Result<List<Experience>>> GetAll()
    {
        try
        {
            var experiences = await _unitOfWork.Experiences.GetAll().ToListAsync();

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