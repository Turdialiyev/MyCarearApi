using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public partial class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompanyService> _logger;

    public CompanyService(IUnitOfWork unitOfWork, ILogger<CompanyService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    public async ValueTask<Result<Company>> CreateCompany(Company company)
    {
        try
        {
            if(company is null)
            return new("Company model can't be null");

            var createdCompany = await _unitOfWork.Companies.AddAsync(ToEntityCompany(company));

            if(createdCompany is null)
            return new("Company is not created");
            
            return new(true) {Data = ToModelCompany(createdCompany)};

        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Compans is not created: {company.Name}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Contact>> CreateCompanyContact(Contact contact)
    {
        try
        {
            if(contact is null)
            return new("contacts can't be null");

            var createdContact = await _unitOfWork.CompanyContacts.AddAsync(ToEntityContact(contact));

            if(createdContact is null)
            return new("contact is not created");

            return new(true) {Data = ToModelContact(createdContact)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Company Contacts is not created: {contact}");
            throw new Exception(e.Message);
        }
    }


    public async ValueTask<Result<CompanyLocation>> CreateCompanyLocation(CompanyLocation companyLocation)
    {
        try
        {
            if(companyLocation is null)
            return new("CompanyLocation can'y be null here");

            var createdLocation = await _unitOfWork.CompanyLocations
            .AddAsync(ToEntityCompanyLocation(companyLocation));

            if(createdLocation is null)
            return new("Company Location is not created");

            return new(true){ Data = ToModelCompanyLocation(createdLocation)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Company Location is not created: {companyLocation}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<string>> UploadCompanyPhoto(IFormFile file)
    {
        try
        {
            if(file is null)
            return new("File Can't be null");
            
            var FilePath = Directory.GetCurrentDirectory()+"/wwwroot";

            var path = Path.Combine(FilePath, file.FileName);

            var createdFile = new FileStream(path, FileMode.Create);
            file.CopyTo(createdFile);

            createdFile.Close();
            
            return new(true)  {Data = path};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"File is not uploaded: {file.Name}");
            throw new Exception(e.Message);
        }
    }
}