using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface ILanguageService
{
    ValueTask<Result<List<UserLanguage>>> GetAll();
    ValueTask<Result<UserLanguage>> Save(int freelancerId, UserLanguage langauge);
    ValueTask<Result<UserLanguage>> Delete(int id, UserLanguage language);
}