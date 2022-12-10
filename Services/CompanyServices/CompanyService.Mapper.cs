using MyCarearApi.Models;

namespace MyCarearApi.Services;


public partial class CompanyService
{
    private Entities.Company ToEntityCompany(Company company, string filePath, string userId)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Photo = filePath,
        AppUserId = userId
    };
    private Company ToModelCompany(Entities.Company company)
    => new()
    {
        Id = company.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Photo = company.Photo,
    };

    private Entities.Company UpdatedCompany(Entities.Company existCopany, string filePath, Company company)
    {
       existCopany.Photo = filePath;
       existCopany.PhoneNumber = company.PhoneNumber;
       existCopany.Name = company.Name;

       return existCopany;
    }
    private Contact ToModelContact(Entities.Contact contact)
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
         AppUserId = contact.AppUserId
         
    };
   

    private Entities.Contact ToEntityContact(Contact contact, string userId)
    => new()
    {
         WatsApp = contact.WatsApp,
         Facebook = contact.Facebook,
         Telegram = contact.Telegram,
         GitHub = contact.GitHub,
         Twitter = contact.Twitter,
         Instagram = contact.Instagram,
         WebSite = contact.WebSite,
         AppUserId = userId
    };
    private Entities.Contact ToEntityUpdateContact(Entities.Contact existContact, Contact contact)
    {
        existContact.Facebook = contact.Facebook;
        existContact.Instagram = contact.Instagram;
        existContact.WatsApp = contact.WatsApp;
        existContact.Telegram = contact.Telegram;
        existContact.GitHub = contact.GitHub;
        existContact.WebSite = contact.WebSite;

        return existContact;
    }

    
    private CompanyLocation ToModelCompanyLocation(Entities.CompanyLocation location)
    => new()
    {
       Id = location.Id,
       Location = location.Location,
       Description = location.Description,
       CompnayId = location.CompanyId
    };


    private Entities.CompanyLocation ToEntityCompanyLocation(CompanyLocation location)
    => new()
    {
       Location = location.Location,
       Description = location.Description,
       CompanyId = location.CompnayId
    };
     private Entities.CompanyLocation ToEntityUpdateLocation(Entities.CompanyLocation existLocation, CompanyLocation companyLocation)
    {
        existLocation.Id = companyLocation.Id;
        existLocation.Description = companyLocation.Description;
        existLocation.Location = companyLocation.Location;
        existLocation.CompanyId = companyLocation.CompnayId;

        return existLocation;
    }
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