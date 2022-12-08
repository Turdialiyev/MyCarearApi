using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;

namespace MyCarearApi.Controllers;

public partial class CompanyController
{
   
    private  ReturnCreatedCompany ToDtoCompany(Company? company)
    => new()
    {
        Id = company!.Id,
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
        Photo = company.Photo,
        ContactId = company.ContactId
    };

    private  Company ToModelCompany(CreateCompanyDto company)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
    };
   
   private CompanyLocation ToModelLocation(CreateCompanyLocation location)
    => new()
    {
        Location = location.Location,
    };
    
    private ReturnCreatedCompanyLocation ToDtoLocation(CompanyLocation? location)
    => new()
    {
        Id = location!.Id,
        CompanyId = location.CompanyId,
        Location = location.Location
    };
    private ReturnCreatedCompanyContact? ToDtoCreatedContact(Contact? contact)
    => new()
    {
       Id = contact.Id,
       WatsApp = contact.WatsApp,
       Instagram = contact.Instagram,
       Facebook = contact.Facebook,
       WebSite = contact.WebSite,
       Telegram = contact.Telegram,
       Twitter = contact.Twitter
    };

    private Contact ToModelCompanyContact(CreateCompanyContact contact)
    => new()
    {
       WatsApp = contact.WatsApp,
       Instagram = contact.Instagram,
       Facebook = contact.Facebook,
       WebSite = contact.WebSite,
       Telegram = contact.Telegram,
       Twitter = contact.Twitter 
    };
 }