using System.Security.Claims;
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

    [HttpPost("Create")]
    public async Task<IActionResult> CreateCompany(CreateCompanyDto createCompany)
    {
        
        if(string.IsNullOrEmpty(createCompany.Email)
        || string.IsNullOrEmpty(createCompany.Name) 
        || string.IsNullOrEmpty(createCompany.Email)
        || string.IsNullOrEmpty(createCompany.PhoneNumber))

        return BadRequest("This Properties can't be null or empty");
        
        var userId = User.Identity.Name;
        System.Console.WriteLine($"================> {userId}");
        
        
        var createdCompany = await _companyService
        .CreateCompany(ToModelCompany(createCompany), userId);

        return Ok(ToDtoCompany(createdCompany.Data));
    }

    [HttpPost("Location/Create")]
    public async Task<IActionResult> CreateLocation(CreateCompanyLocation location)
    {
      if(location.Locations is null)
      return BadRequest("Location can't be null or empty");

     var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.ToString()??"userid";

     var createdLocation = await _companyService
      .CreateCompanyLocation(ToModelLocation(location, "userid"));
      

      return Ok(ToDtoLocation(createdLocation.Data));
    }

    [HttpPost("Contact/Create")]
    public async Task<IActionResult> CreateCompanyContact(CreateCompanyContact contact)
    {
     var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.ToString()??"userid";

     var createdContact = await _companyService
     .CreateCompanyContact(ToModelCompanyContact(contact),userId);

     return Ok(ToDtoCreatedContact(createdContact.Data));
    }
    
   

    [HttpPost("User/Create")]
    public async Task<IActionResult> CreateCompanyUser(CompanyUser companyUser)
    {
      if(string.IsNullOrEmpty(companyUser.CopmanyEmail)
       ||string.IsNullOrEmpty(companyUser.PhoneNumber)
       ||string.IsNullOrEmpty(companyUser.FirstName)
       ||string.IsNullOrEmpty(companyUser.LastName))
       return BadRequest();
       
       var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.ToString()??"userid";

       var createdUser = await _companyService.CreateCopmanyUser(ToModelCompanyUser(companyUser), userId);

       return Ok(ToDtoCompanyUser(createdUser.Data));

    }

   
}