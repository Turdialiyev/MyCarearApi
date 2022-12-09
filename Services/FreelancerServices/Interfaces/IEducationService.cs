using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IEducationService
{
    ValueTask<Result<List<Education>>> GetAll();
    ValueTask<Result<Education>> Save(int freelncerId, Education education);
    ValueTask<Result<Education>> Update(int educationId, Education education);
    ValueTask<Result<Education>> Delete(int educationId, Education education);
}