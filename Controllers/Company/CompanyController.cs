using Microsoft.AspNetCore.Mvc;
using MyCarearApi.Dtos;
using MyCarearApi.Models;
using MyCarearApi.Services;

namespace MyCarearApi.Controllers;

[ApiController]
[Route("[api/controller]")]

public partial class CompanyController : ControllerBase
{
    private readonly ICompanyService _companyService;

    public CompanyController(ICompanyService companyService)
    {
        _companyService = companyService;
    }

    [HttpPost("Company}")]
    public async Task<IActionResult> CreateCompany(CreateCompanyDto createCompany)
    {
        if(string.IsNullOrEmpty(createCompany.Email)
        || string.IsNullOrEmpty(createCompany.Name) 
        || string.IsNullOrEmpty(createCompany.Email)
        || string.IsNullOrEmpty(createCompany.PhoneNumber))

        return BadRequest("This Properties can't be null or empty");

        var createdCompany = await _companyService
        .CreateCompany(ToModelCompany(createCompany));

        return Ok(ToDtoCompany(createdCompany.Data));
    }

    
}