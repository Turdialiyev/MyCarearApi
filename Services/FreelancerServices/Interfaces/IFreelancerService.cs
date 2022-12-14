using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<List<FreelancerInformation>>> GetAll();
    ValueTask<Result<FreelancerInformation>> GetById(int freelancerId);
    ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image);
    ValueTask<Result<FreelancerInformation>> FinishResume(int freelancerId, bool finish);
    ValueTask<Result<FreelancerInformation>>  Resume(int freelancerId, TypeResume resume);
    ValueTask<Result<FreelancerInformation>> Address(string userId, Address address);
    ValueTask<Result<FreelancerInformation>> Position(string userId, Position possion);
    ValueTask<Result<FreelancerInformation>> Contact(string userId, FreelancerContact contacts);
}