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
    public ValueTask<Result<Education>> Delete(int educationId, Education education)
    {
        try
        {
            var existEducation = _unitOfWork.
            if (existEducation is null)
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

    public ValueTask<Result<List<Education>>> GetAll()
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Education>> Save(int freelncerId, Education education)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Education>> Update(int educationId, Education education)
    {
        throw new NotImplementedException();
    }
}