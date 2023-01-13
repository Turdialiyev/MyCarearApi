using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<List<FreelancerInformation>>> GetAll();
    Result<IEnumerable<Entities.FreelancerInformation>> GetByPage(int page, int size);
    ValueTask<Result<FreelancerInformation>> GetById(string userId);
    ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image);
    ValueTask<Result<FreelancerInformation>> FinishResume(string userId, bool finish);
    ValueTask<Result<FreelancerInformation>>  Resume(string userId, int resume);
    ValueTask<Result<FreelancerInformation>> Address(string userId, Address address);
    ValueTask<Result<FreelancerInformation>> Position(string userId, Position possion);
    ValueTask<Result<FreelancerInformation>> Contact(string userId, FreelancerContact contacts);
    int GetCount();
}