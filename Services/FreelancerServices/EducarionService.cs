# pragma warning disable
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class EducationService : IEducationService
{
    private readonly ILogger<EducationService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.AppUser> _userManager;

    public EducationService(
        ILogger<EducationService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
    }
    public async ValueTask<Result<Education>> Delete(int educationId)
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
        FreelancerInformationId = result.FreelancerInformationId
    };

    public async ValueTask<Result<List<Education>>> GetAll(string userId)
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

            var educations = await _unitOfWork.Educations.GetAll().Where(x => x.FreelancerInformationId == existInformation.Id).ToListAsync();

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

    public async ValueTask<Result<Education>> Save(string userId, Education education)
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

            var result = await _unitOfWork.Educations.AddAsync(ToEntity(education, existInformation.Id));

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
        FreelancerInformationId = freelncerId
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