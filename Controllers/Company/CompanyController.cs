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
    [Authorize]
    public async Task<IActionResult> CreateUser(CompanyUser companyUser)
    {
      if(string.IsNullOrEmpty(companyUser.CopmanyEmail)
      ||string.IsNullOrEmpty(companyUser.LastName)
      ||string.IsNullOrEmpty(companyUser.FirstName)
      ||string.IsNullOrEmpty(companyUser.PhoneNumber))
      return BadRequest();

      System.Console.WriteLine("==========>");

      var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      System.Console.WriteLine("==========>" + userId);


      var createdUser = await _companyService
      .CrateCompanyUser(ToModelCompanyUser(companyUser), userId);

      return Ok(createdUser);
    }


    [HttpPost("Create")]
    public async Task<IActionResult> CreateCompany([FromForm]CreateCompanyDto createCompany)
    {
        if(string.IsNullOrEmpty(createCompany.PhoneNumber)
        || string.IsNullOrEmpty(createCompany.Name)
        || string.IsNullOrEmpty(createCompany.PhoneNumber))

        return BadRequest("This Properties can't be null or empty");

        var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
        // default holatdagi foydalaniladigan userI {50089621-aa51-4f53-9860-bf5b4b497dfc}
        var createdCompany = await _companyService
        .CreateCompany(ToModelCompany(createCompany), userId, createCompany.File);

        return Ok(ToDtoCompany(createdCompany.Data));
    }

    [HttpPost("Location/Create")]
    public async Task<IActionResult> CreateLocation(CreateCompanyLocation location)
    {
      if(location.Location is null)
      return BadRequest("Location can't be null or empty");
      
      // var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

      var returnLocation = new ReturnCreatedCompanyLocation();
      
      for (int i = 0; i < location.Location.Count(); i++)
      {
        var createdLocation = await _companyService.CreateCompanyLocation(
            ToModelLocation(location.Location[i],location.Description!), "e070a46f-6bac-433f-bd9f-fc9c2dc37c37");
            
            
            returnLocation.Locations?.Add(createdLocation.Data!.Location!);
            returnLocation.Description = createdLocation.Data.Description;
            returnLocation.UserId = createdLocation.Data.UserId;
      }
      
      return Ok(returnLocation);
      
    }

    [HttpPost("Contact/Create")]
    public async Task<IActionResult> CreateCompanyContact(CreateCompanyContact contact)
    {
     var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
     var createdContact = await _companyService
     .CreateCompanyContact(ToModelCompanyContact(contact), userId);

     return Ok(ToDtoCreatedContact(createdContact.Data));
    }
    

}