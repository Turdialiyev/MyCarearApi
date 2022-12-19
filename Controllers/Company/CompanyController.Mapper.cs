using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Models;
using MyCarearApi.Dtos;

namespace MyCarearApi.Controllers;

public partial class CompanyController
{
   
    private  ReturnCreatedCompany ToDtoCompany(Company? company)
    => new()
    {
        Id = company!.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Photo = company.Photo,
    };

    private  Company ToModelCompany(CreateCompanyDto company)
    => new()
    {
        Id = company.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber
    };
   
   private CompanyLocation ToModelLocation(string location, int id, string description, int companyId)
    => new()
    {
        CompnayId = companyId,
        Location = location,
        Description = description,
        Id = id
    };
    
   
    private ReturnCreatedCompanyContact? ToDtoCreatedContact(Contact? contact)
    => new()
    {
       Id = contact.Id,
       WatsApp = contact.WatsApp,
       Instagram = contact.Instagram,
       Facebook = contact.Facebook,
       WebSite = contact.WebSite,
       GitHub = contact.GitHub,
       Telegram = contact.Telegram,
       Twitter = contact.Twitter,
       
    };

    private Contact ToModelCompanyContact(CreateCompanyContact contact)
    => new()
    {
       Id = contact.Id,
       WatsApp = contact.WatsApp,
       Instagram = contact.Instagram,
       Facebook = contact.Facebook,
       WebSite = contact.WebSite,
       GitHub = contact.GitHub,
       Telegram = contact.Telegram,
       Twitter = contact.Twitter,
       
    };

    private AppUser ToModelCompanyUser(CompanyUser companyUser)
    => new()
    {
        FirstName = companyUser.FirstName,
        LastName = companyUser.LastName,
        CopmanyEmail = companyUser.CopmanyEmail,
        PhoneNumber = companyUser.PhoneNumber
    };
 }