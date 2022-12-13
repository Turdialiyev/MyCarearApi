
using MyCarearApi.Entities;

namespace MyCarearApi.Services;

public partial class FreelancerService
{

    private Models.Address ToModelAdress(Entities.Address? existAddress) => new()
    {
        Id = existAddress!.Id,
        CountryId = existAddress.CountryId,
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
        Id = freelancerInformation.Id,
        FirstName = freelancerInformation.FirstName,
        LastName = freelancerInformation.LastName,
        Email = freelancerInformation.Email,
        PhoneNumber = freelancerInformation.PhoneNumber,
        FreelancerImage = freelancerInformation.FreelancerImage,
        PossionId = freelancerInformation.PositionId,
        Birthday = freelancerInformation.Birthday,
        TypeResume = freelancerInformation.TypeResume,
        Finish = freelancerInformation.Finish,
        FreelancerSkills = freelancerInformation.FreelancerSkills!.Select(x =>
            new Models.FreelancerSkill
            {
                Id = x.Id,
                SkillId = x.SkillId,
                Skill = new Models.Skill
                {
                    Id = x.Skill!.Id,
                    Name = x.Skill!.Name,
                    PositionId = x.Skill.PositionId,
                },
            }),
        FreelancerHobbies = freelancerInformation.Hobbies!.Select(x =>
            new Models.FreelancerHobby
            {
                Id = x.Id,
                Hobby = new Models.Hobby
                {
                    Id = x.Hobby!.Id,
                    Name = x.Hobby!.Name,
                }
            }),
        UserLanguages = freelancerInformation.UserLanguages!.Select(x => 
            new Models.UserLanguage 
            {
                LanguageId = x.LanguageId,
                Level = x.Level,
                Name = x.Language!.Name,
            })    


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
        PossionId = existFreelancer.PositionId,

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
        WebSite = contacts.WebSite,
        FreelancerInformationId = contacts.FreelancerInformationId,
    };

    private Entities.FreelancerContact ToEntityContact(Models.FreelancerContact contacts, int freelancerId) => new()
    {
        WebSite = contacts.WebSite,
        Facebook = contacts.Facebook,
        Instagram = contacts.Instagram,
        Telegram = contacts.Telegram,
        GitHub = contacts.GitHub,
        Twitter = contacts.Twitter,
        WatsApp = contacts.WatsApp,
        FreelancerInformationId = freelancerId,
    };
}