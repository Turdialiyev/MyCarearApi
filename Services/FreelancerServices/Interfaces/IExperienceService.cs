using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IExperienceService 
{
    ValueTask<Result<List<Experience>>> GetAll();
    ValueTask<Result<Experience>> Save(int freelncerId, Experience experience);
    ValueTask<Result<Experience>> Update(int experienceId, Experience experience);
    ValueTask<Result<Experience>> Delete(int experienceId, Experience experience);
}