using MyCarearApi.Dtos;
using MyCarearApi.Models;

namespace MyCarearApi.Controllers;

public partial class CompanyController
{
   
    private ReturnCreatedCompany ToDtoCompany(Company? company)
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

    private Company ToModelCompany(CreateCompanyDto company)
    => new()
    {
        Name = company.Name,
        PhoneNumber = company.PhoneNumber,
        Email = company.Email,
        Description = company.Description,
    };


 }