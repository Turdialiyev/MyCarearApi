using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class LanguageService : ILanguageService
{
    private ILogger<LanguageService> _logger;
    private IUnitOfWork _unitOfWork;

    public LanguageService(ILogger<LanguageService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }
    public async ValueTask<Result<UserLanguage>> Delete(int id)
    {
        try
        {
            var existLanguage = _unitOfWork.UserLanguages.GetById(id);
            
            if (existLanguage is null)
                return new("Language with given Id Not Found");

            var result = await _unitOfWork.UserLanguages.Remove(existLanguage);

            return new(true) { Data = ToModel(result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(LanguageService)}", e);
            throw new("Couldn't delete Langauge. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<List<UserLanguage>>> GetAll()
    {
        try
        {
            var languages = await _unitOfWork.UserLanguages.GetAll().ToListAsync();

            if (languages is null)
                return new(false) { ErrorMessage = "Any language not found" };

            return new(true) { Data = languages.Select(ToModel).ToList() };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(LanguageService)} .");
            throw new("Couldn't get Languages Please contact support");
        }
    }

    public async ValueTask<Result<UserLanguage>> Save(int freelancerId, UserLanguage langauge)
    {
        if (langauge is null)
            return new("Langauge null exseption");

        try
        {
            var exsitFreelancer = _unitOfWork.FreelancerInformations.GetById(freelancerId);

            if (exsitFreelancer is null)
                return new("Freelancer is not found");
            
            var result = await _unitOfWork.UserLanguages.AddAsync(ToEntity(langauge, freelancerId));

            return new(true) { Data = ToModel(result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(LanguageService)}", e);

            throw new("Couldn't create Language. Plaese contact support", e);
        }
    }

    private Entities.UserLanguage ToEntity(UserLanguage result, int id) => new()
    {
        LanguageId = result.LanguageId,
        Level = result.Level,
        FreelancerInformationId = id
    };

    private UserLanguage ToModel(Entities.UserLanguage result) => new()
    {
        Id = result.Id,
        LanguageId = result.LanguageId,
        Level = result.Level,
      
    };
}