# pragma warning disable
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("api/[controller]")]

public partial class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }
    
    [HttpPost("User/Create")]
 
    public async Task<IActionResult> CreateUser(CompanyUser companyUser)
    {
      if(string.IsNullOrEmpty(companyUser.CopmanyEmail)
      ||string.IsNullOrEmpty(companyUser.LastName)
      ||string.IsNullOrEmpty(companyUser.FirstName)
      ||string.IsNullOrEmpty(companyUser.PhoneNumber))
      return BadRequest("This properties can't be null");


      var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

      var createdUser = await _companyService
      .CrateCompanyUser(ToModelCompanyUser(companyUser), userId);

      return Ok(createdUser);
    }


    [HttpPost("Create")]
    public async Task<IActionResult> CreateCompany([FromForm] CreateCompanyDto createCompany)
    {
        if(string.IsNullOrEmpty(createCompany.PhoneNumber)
        || string.IsNullOrEmpty(createCompany.Name)
        || string.IsNullOrEmpty(createCompany.PhoneNumber))

        return BadRequest("This Properties can't be null or empty");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        // default holatdagi foydalaniladigan userId {50089621-aa51-4f53-9860-bf5b4b497dfc}
        var createdCompany = await _companyService
        .CreateCompany(ToModelCompany(createCompany), userId, createCompany.File);

        return Ok(ToDtoCompany(createdCompany.Data));
    }

    [HttpPost("Location/Create")]
    public async Task<IActionResult> CreateLocation(CreateCompanyLocation location)
    {
      if(location.Locations is null)
      return BadRequest("Location can't be null or empty");
      
     
      var notEmptyLocationsId = new List<int>();
      for (int i = 0; i < location.Locations.Count(); i++)
      {
         var createdLocation = await _companyService.CreateCompanyLocation(
         ToModelLocation(location.Locations[i].Location!,
         location.Locations[i].Id, location.Description!, location.CompnayId));
     
        location.CompnayId = createdLocation.Data.CompnayId;
        location.Description = createdLocation.Data.Description;
        location.Locations[i].Id = createdLocation.Data.Id;
        location.Locations[i].Location = createdLocation.Data.Location;
         
        
        if(location.Locations[i].Id != 0)
        {
           notEmptyLocationsId.Add(location.Locations[i].Id);
        }
      }

       
       if(notEmptyLocationsId.Count() > 0)
       {
          _companyService.DeleteEmptyLocations(notEmptyLocationsId, location.CompnayId);
       }

      return Ok(location);
    }

    [HttpPost("Contact/Create")]
    public async Task<IActionResult> CreateCompanyContact(CreateCompanyContact contact)
    {
     var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
     var createdContact = await _companyService
     .CreateCompanyContact(ToModelCompanyContact(contact), userId);

     return Ok(ToDtoCreatedContact(createdContact.Data));
    }
    

}