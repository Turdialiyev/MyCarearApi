using Microsoft.AspNetCore.Identity;
using MyCarearApi.Models;

namespace MyCarearApi.Services;


public partial class CompanyService
{
    private Entities.Company ToEntityCompany(Company company,string userId,string filePath)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
        Photo = filePath,
        AppUserId = userId
    };
    private Company ToModelCompany(Entities.Company company)
    => new()
    {
        Id = company.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
        Photo = company.Photo,
        AppUserId = company.AppUserId
    };
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
         UserId = contact.UserId
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
         UserId = contact.UserId
    };

    
    private CompanyLocation ToModelCompanyLocation(List<string> locations, string AppUserId, string description)
    => new()
    {
       Locations = locations,
       AppUserId = AppUserId,
       Description = description
    };


    private Entities.CompanyLocation ToEntityCompanyLocation(string location, string userId, string description)
    => new()
    {
       Location = location,
       Description = description,
       UserId = userId
    };

    private AppUser ToModelUser(Entities.AppUser? identityUser)
    => new()
    {
       FirstName = identityUser.FirstName,
       LastName = identityUser.LastName,
       CopmanyEmail = identityUser.CompanyEmail,
       PhoneNumber = identityUser.PhoneNumber
    };


}