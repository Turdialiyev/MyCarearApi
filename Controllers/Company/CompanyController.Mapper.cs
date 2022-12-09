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
        ContactId = company.ContactId,
    };

    private  Company ToModelCompany(CreateCompanyDto company)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
        AppUserId = ""
    };
   
   private CompanyLocation ToModelLocation(CreateCompanyLocation location, string userId)
    => new()
    {
        Locations = location.Locations,
        Description = location.Description,
        AppUserId = userId
        
    };
    
    private ReturnCreatedCompanyLocation ToDtoLocation(CompanyLocation? location)
    => new()
    {
        AppUserId = location.AppUserId,
        Location = location.Locations
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

    private CompanyUser? ToDtoCompanyUser(AppUser? user)
    => new()
    {
        FirstName = user.FirstName,
        CopmanyEmail = user.CopmanyEmail,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber
    };

    private AppUser ToModelCompanyUser(CompanyUser user)
    => new()
    {
        FirstName = user.FirstName,
        CopmanyEmail = user.CopmanyEmail,
        LastName = user.LastName,
        PhoneNumber = user.PhoneNumber
    };
 }