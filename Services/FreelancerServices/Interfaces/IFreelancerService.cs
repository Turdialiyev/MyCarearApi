using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<FreelancerInformation>> Information(int userId, FreelancerInformation information, IFormFile image);
    ValueTask<Result<Address>> Address(int freelancerId, Address address);
    ValueTask<Result<FreelancerInformation>> Position(int freelancerId, Position possion);
    ValueTask<Result<UserLanguage>> UserLanguage(int freelancerId, List<UserLanguage> userLanguage);
    ValueTask<Result<FreelancerContact>> Contact(int freelancerId, FreelancerContact contacts);
}