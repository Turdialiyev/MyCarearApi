using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IFreelancerService
{
    ValueTask<Result<FreelancerInformation>> Information(FreelancerInformation information, IFormFile image);
    ValueTask<Result<Address>> Address(int freelancerId, Address address);
    ValueTask<Result<Position>> Position(int freelancerId, Position possion);
    ValueTask<Result<UserLanguage>> UserLanguage(int freelancerId, List<UserLanguage> userLanguage);
    ValueTask<Result<Experience>> WorkExperience(int freelancerId, List<Experience> experiences);
    ValueTask<Result<Education>> Education(int freelancerId, List<Education> educations);
    ValueTask<Result<Contact>> Contact(int freelancerId, List<Contact> contacts);
}