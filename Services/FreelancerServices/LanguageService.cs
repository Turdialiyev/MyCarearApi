using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class LanguageService : ILanguageService
{
    private ILogger<LanguageService> _logger;
    private IUnitOfWork _unitOfWork;
    private UserManager<Entities.AppUser> _userManager;

    public LanguageService(
        ILogger<LanguageService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
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

    public async ValueTask<Result<List<UserLanguage>>> GetAll(string userId)
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

            var languages = await _unitOfWork.UserLanguages.GetAll().Where(x => x.FreelancerInformationId == existInformation.Id).Include(x => x.Language).ToListAsync();

            if (!languages.Any())
                return new(false) { ErrorMessage = "Any language not found" };

            return new(true) { Data = languages.Select(ToModelGet).ToList() };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(LanguageService)} . {e}");
            throw new("Couldn't get Languages Please contact support");
        }
    }

    public async ValueTask<Result<UserLanguage>> Save(string userId, UserLanguage langauge)
    {

        try
        {
            if (langauge is null)
                return new("Langauge null exseption");

            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "User Id not found" };

            var existInformation = _unitOfWork.FreelancerInformations.GetAll().FirstOrDefault(f => f.AppUserId == userId);

            if (existInformation is null)
                return new(false) { ErrorMessage = "Freelancer information not found" };

            var result = await _unitOfWork.UserLanguages.AddAsync(ToEntity(langauge, existInformation.Id));

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

    private UserLanguage ToModelGet(Entities.UserLanguage result) => new()
    {
        Id = result.Id,
        LanguageId = result.LanguageId,
        Name = result.Language!.Name,
        Level = result.Level,
    };
}