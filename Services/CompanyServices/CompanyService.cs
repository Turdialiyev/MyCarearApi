using Microsoft.AspNetCore.Identity;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public partial class CompanyService : ICompanyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CompanyService> _logger;
    private readonly UserManager<Entities.AppUser> _userManager;

    public CompanyService(IUnitOfWork unitOfWork,
     ILogger<CompanyService> logger,
     UserManager<Entities.AppUser> userManager)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _userManager = userManager;
    }
    public async ValueTask<Result<Company>> CreateCompany(Company company, string userId)
    {
        try
        {
            if(company is null)
            return new("Company model can't be null");


            var createdCompany = await _unitOfWork.Companies.AddAsync(ToEntityCompany(company, userId,"nimadir"));

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

    public async ValueTask<Result<Contact>> CreateCompanyContact(Contact contact, string userId)
    {
        try
        {
            if(contact is null)
            return new("contacts can't be null");

            var createdContact = await _unitOfWork.CompanyContacts.AddAsync(ToEntityContact(contact,userId));

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
            
            List<string> locations = new List<string>();

            for (int i = 0; i < companyLocation.Locations!.Count(); i++)
            {
                var createdLocation =  await _unitOfWork.CompanyLocations.AddAsync(
                                        ToEntityCompanyLocation(companyLocation.Locations[i], 
                                        companyLocation!.AppUserId!, companyLocation?.Description!));

                locations.Add(createdLocation.Location!);
            }

            
            
            
            // var createdLocation =  await _unitOfWork.CompanyLocations
            // .AddAsync(ToEntityCompanyLocation(companyLocation));
             

            if(locations is null)
            return new("Company Location is not created");

            return new(true){ Data = ToModelCompanyLocation(locations, companyLocation.AppUserId, companyLocation.Description)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Company Location is not created: {companyLocation}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<AppUser>> CreateCopmanyUser(AppUser user, string userId)
    {
        try
        {
            var appUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);
            
            await _userManager.UpdateAsync(appUser);

            var identityUser = _userManager.Users.FirstOrDefault(x => x.Id == userId);

            return new(true){Data = ToModelUser(identityUser)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"User not created");
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