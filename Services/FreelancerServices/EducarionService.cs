using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class EducationService : IEducationService
{
    private readonly ILogger<EducationService> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public EducationService(ILogger<EducationService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async ValueTask<Result<Education>> Delete(int educationId, Education education)
    {
        try
        {
            var existEducation = _unitOfWork.Educations.GetById(educationId);
            if (existEducation is null)
                return new("Experience with given Id Not Found");

            var result = await _unitOfWork.Educations.Remove(existEducation);

            return new(true) { Data = ToModel(result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ExperienceService)}", e);
            throw new("Couldn't delete Education. Plaese contact support", e);

        }
    }

    private Education ToModel(Entities.Education result) => new()
    {
        Id = result.Id,
        SchoolName = result.SchoolName,
        EducationDegree = result.EducationDegree,
        TypeStudy = result.TypeStudy,
        StartDate = result.StartDate,
        EndDate = result.EndDate,
        CurrentStudy = result.CurrentStudy,
        Location = result.Location,
        FrelancerInfoId = result.FrelancerInfoId
    };

    public async ValueTask<Result<List<Education>>> GetAll()
    {
        try
        {
            var educations = await _unitOfWork.Educations.GetAll().ToListAsync();

            if (educations is null)
                return new(false) { ErrorMessage = "Any Education not found" };

            return new(true) { Data = educations.Select(ToModel).ToList() };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(EducationRepository)} .");
            throw new("Couldn't get Educations Please contact support");
        }
    }

    public async ValueTask<Result<Education>> Save(int freelncerId, Education education)
    {
        if (education is null)
            return new("Education null exseption");

        try
        {
            var exsitFreelancer = _unitOfWork.FreelancerInformations.GetById(freelncerId);

            if (exsitFreelancer is null)
                return new("exsitFreelancer is not found");

            var result = await _unitOfWork.Educations.AddAsync(ToEntity(education, freelncerId));

            return new(true) { Data = ToModel(result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(EducationService)}", e);

            throw new("Couldn't create Education. Plaese contact support", e);
        }
    }

    private Entities.Education ToEntity(Education model, int freelncerId) => new()
    {
        SchoolName = model.SchoolName,
        EducationDegree = model.EducationDegree,
        TypeStudy = model.TypeStudy,
        StartDate = model.StartDate,
        EndDate = model.EndDate,
        CurrentStudy = model.CurrentStudy,
        Location = model.Location,
        FrelancerInfoId = freelncerId
    };

    public async ValueTask<Result<Education>> Update(int educationId, Education education)
    {
        try
        {

            if (educationId == 0)
                return new("Given Education Id invalid");

            if (education is null)
                return new("Education null");

            var existEducation = _unitOfWork.Educations.GetById(educationId);

            if (existEducation is null)
                return new("Education given Id not found");

            existEducation.SchoolName = education.SchoolName;
            existEducation.EducationDegree = education.EducationDegree;
            existEducation.TypeStudy = education.TypeStudy;
            existEducation.StartDate = education.StartDate;
            existEducation.EndDate = education.EndDate;
            existEducation.CurrentStudy = education.CurrentStudy;
            existEducation.Location = education.Location;

            var updated = await _unitOfWork.Educations.Update(existEducation);

            return new(true) { Data = ToModel(updated) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(EducationService)}");
            throw new("Couldn't update Education. Please contact support", e);
        }
    }
}