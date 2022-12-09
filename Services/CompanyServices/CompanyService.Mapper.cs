using MyCarearApi.Models;

namespace MyCarearApi.Services;


public partial class CompanyService
{
    private Entities.Company ToEntityCompany(Company company, string filePath)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Photo = filePath
    };
    private Company ToModelCompany(Entities.Company company)
    => new()
    {
        Id = company.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Photo = company.Photo,
    };
    private Contact ToModelContact(Entities.Contact contact)
    => new()
    {
         WatsApp = contact.WatsApp,
         Facebook = contact.Facebook,
         Telegram = contact.Telegram,
         GitHub = contact.GitHub,
         Twitter = contact.Twitter,
         Instagram = contact.Instagram,
         WebSite = contact.WebSite,
         AppUserId = contact.UserId
         
    };
   

    private Entities.Contact ToEntityContact(Contact contact, string userId)
    => new()
    {
         Id = contact.Id,
         WatsApp = contact.WatsApp,
         Facebook = contact.Facebook,
         Telegram = contact.Telegram,
         GitHub = contact.GitHub,
         Twitter = contact.Twitter,
         Instagram = contact.Instagram,
         WebSite = contact.WebSite,
         UserId = userId
    };

    
    private CompanyLocation ToModelCompanyLocation(Entities.CompanyLocation location)
    => new()
    {
       Location = location.Location,
       UserId = location.UserId
    };


    private Entities.CompanyLocation ToEntityCompanyLocation(CompanyLocation location)
    => new()
    {
       Location = location.Location,
       Description = location.Description,
       UserId = location.UserId
    };
    private Entities.AppUser ToEntityCompanyUser(AppUser user, Entities.AppUser? identityUser)
    {
        identityUser.CopmanyEmail = user.CopmanyEmail;
        identityUser.FirstName = user.FirstName;
        identityUser.LastName = user.LastName;
        identityUser.PhoneNumber = user.PhoneNumber;
        return identityUser;
    } 


    private AppUser ToModelUser(Entities.AppUser? user)
    => new()
    {
       FirstName = user.FirstName,
       LastName = user.LastName,
       CopmanyEmail = user.CopmanyEmail,
       PhoneNumber = user.PhoneNumber
    };

}