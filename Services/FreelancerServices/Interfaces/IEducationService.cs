using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IEducationService
{
    ValueTask<Result<List<Education>>> GetAll(string userId);
    ValueTask<Result<Education>> Save(string userId, Education education);
    ValueTask<Result<Education>> Update(int educationId, Education education);
    ValueTask<Result<Education>> Delete(int educationId);
}