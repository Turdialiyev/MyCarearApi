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

        var CompanyName = User.Identity!.Name;
        var createdCompany = await _companyService
        .CreateCompany(ToModelCompany(createCompany));

        return Ok(ToDtoCompany(createdCompany.Data));
    }

    [HttpPost("Location/Create")]
    public async Task<IActionResult> CreateLocation(CreateCompanyLocation location)
    {
      if(string.IsNullOrEmpty(location.Location))
      return BadRequest("Location can't be null or empty");

      var createdLocation = await _companyService
      .CreateCompanyLocation(ToModelLocation(location));

      return Ok(ToDtoLocation(createdLocation.Data));
    }

    [HttpPost("Contact/Create")]
    public async Task<IActionResult> CreateCompanyContact(CreateCompanyContact contact)
    {
     var createdContact = await _companyService
     .CreateCompanyContact(ToModelCompanyContact(contact));

     return Ok(ToDtoCreatedContact(createdContact.Data));
    }
    
    [HttpPost("Photo/Create")]
    public async Task<IActionResult> UploadPhoto(IFormFile file)
    {
        if(file is null)
        return BadRequest();
        
        var filePath = await _companyService.UploadCompanyPhoto(file);
        return Ok(filePath.Data);
    }
}