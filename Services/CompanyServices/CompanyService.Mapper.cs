using MyCarearApi.Models;

namespace MyCarearApi.Services;


public partial class CompanyService
{
    private Entities.Company ToEntityCompany(Company company)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
        Photo = company.Photo,
        ContactId = company.ContactId,
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
        ContactId = company.ContactId,
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
         WebSite = contact.WebSite
    };
   

    private Entities.Contact ToEntityContact(Contact contact)
    => new()
    {
         Id = contact.Id,
         WatsApp = contact.WatsApp,
         Facebook = contact.Facebook,
         Telegram = contact.Telegram,
         GitHub = contact.GitHub,
         Twitter = contact.Twitter,
         Instagram = contact.Instagram,
         WebSite = contact.WebSite
    };

    
    private CompanyLocation ToModelCompanyLocation(Entities.CompanyLocation location)
    => new()
    {
       Id = location.Id,
       Location = location.Location,
       CompanyId = location.CompanyId
    };


    private Entities.CompanyLocation ToEntityCompanyLocation(CompanyLocation location)
    => new()
    {
       Location = location.Location,
       CompanyId = location.CompanyId
    };

}