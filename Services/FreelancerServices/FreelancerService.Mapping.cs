
using MyCarearApi.Entities;

namespace MyCarearApi.Services;

public partial class FreelancerService
{

    private Models.Address ToModelAdress(Entities.Address? existAddress) => new()
    {
        CountryId = existAddress!.CountryId,
        RegionId = existAddress.RegionId,
        Home = existAddress.Home,
        FrelancerInformationId = existAddress.FrelancerInformationId
    };

    private Entities.Address ToEntityAddress(int freelancerId, Models.Address address) => new()
    {
        CountryId = address.CountryId,
        RegionId = address.RegionId,
        Home = address.Home,
        FrelancerInformationId = freelancerId
    };
    private Models.FreelancerInformation ToModel(Entities.FreelancerInformation freelancerInformation) => new()
    {
        FirstName = freelancerInformation.FirstName,
        LastName = freelancerInformation.LastName,
        Email = freelancerInformation.Email,
        PhoneNumber = freelancerInformation.PhoneNumber,
        FreelancerImage = freelancerInformation.FreelancerImage,
    };

    private Entities.FreelancerInformation ToEntity(Models.FreelancerInformation information, string filePath, string userId) => new()
    {
        FirstName = information.FirstName,
        LastName = information.LastName,
        Email = information.Email,
        PhoneNumber = information.PhoneNumber,
        FreelancerImage = filePath,
        AppUserId = userId
    };


    private Models.FreelancerInformation ToModelExistFreelancer(Entities.FreelancerInformation existFreelancer)
    => new()
    {
        Birthday = existFreelancer.Birthday,
        Description = existFreelancer.Description,
        possionId = existFreelancer.PositionId,
        FreelancerSkills = existFreelancer.FreelancerSkills!.Select(x =>
            new Models.FreelancerSkill
            {
                SkillId = x.SkillId,
                FrelanceInformationId = x.FrelanceInformationId
            }),
        Hobbies = existFreelancer.Hobbies!.Select(x =>
            new Models.Hobby
            {
                HobbyId = x.HobbyId,
                FreelanceInformationId = x.FreelanceInformationId,
            })
    };

    private Models.FreelancerContact ToModelContact(Entities.FreelancerContact? contacts) => new()
    {
        Id = contacts!.Id,
        Facebook = contacts.Facebook,
        Instagram = contacts.Instagram,
        Telegram = contacts.Telegram,
        GitHub = contacts.GitHub,
        Twitter = contacts.Twitter,
        WatsApp = contacts.WatsApp,
    };

    private Entities.FreelancerContact ToEntityContact(Models.FreelancerContact contacts, int freelancerId) => new()
    {
        Facebook = contacts.Facebook,
        Instagram = contacts.Instagram,
        Telegram = contacts.Telegram,
        GitHub = contacts.GitHub,
        Twitter = contacts.Twitter,
        WatsApp = contacts.WatsApp,
        FreelancerInformationId = freelancerId,
    };
}