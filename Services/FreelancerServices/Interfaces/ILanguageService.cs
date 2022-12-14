using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface ILanguageService
{
    ValueTask<Result<List<UserLanguage>>> GetAll(string userId);
    ValueTask<Result<UserLanguage>> Save(string userId, UserLanguage langauge);
    ValueTask<Result<UserLanguage>> Delete(int id);
}