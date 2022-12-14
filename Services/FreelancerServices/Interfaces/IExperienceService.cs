using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IExperienceService 
{
    ValueTask<Result<List<Experience>>> GetAll(string userId);
    ValueTask<Result<Experience>> Save(string userId, Experience experience);
    ValueTask<Result<Experience>> Update(int experienceId, Experience experience);
    ValueTask<Result<Experience>> Delete(int experienceId);
}