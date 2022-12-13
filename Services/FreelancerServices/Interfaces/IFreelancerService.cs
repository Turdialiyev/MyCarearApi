using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<List<FreelancerInformation>>> GetAll();
    ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image);
    ValueTask<Result<FreelancerInformation>> FinishResume(int freelancerId, bool finish);
    ValueTask<Result<FreelancerInformation>>  Resume(int freelancerId, TypeResume resume);
    ValueTask<Result<FreelancerInformation>> Address(int freelancerId, Address address);
    ValueTask<Result<FreelancerInformation>> Position(int freelancerId, Position possion);
    ValueTask<Result<FreelancerInformation>> Contact(int freelancerId, FreelancerContact contacts);
}