using MyCarearApi.Entities.Enums;
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image);
    ValueTask<Result<FreelancerInformation>>  Resume(int freelancerId, TypeResume resume);
    ValueTask<Result<Address>> Address(int freelancerId, Address address);
    ValueTask<Result<Position>> Position(int freelancerId, Position possion);
    ValueTask<Result<FreelancerContact>> Contact(int freelancerId, FreelancerContact contacts);
}