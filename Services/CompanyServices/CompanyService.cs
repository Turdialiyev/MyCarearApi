using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;
using MyCareerApi.Entities;

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

    public  async ValueTask<Result<AppUser>> CrateCompanyUser(AppUser user, string userId)
    {
        try
        {
            if(user is null)
            return new("user cant be null");

            var identityUser = await _userManager.FindByIdAsync(userId);
            
            var result = await _userManager.UpdateAsync(ToEntityCompanyUser(user,identityUser));
            if(!result.Succeeded)
            {
                result.Errors.ToList().ForEach(x => {
                    System.Console.WriteLine("User Update Error: " + x.Code + "====>" + x.Description);
                });
            }
            var updatedUser = await _userManager.FindByIdAsync(userId);

            return new(true) {Data = ToModelUser(updatedUser)};

        }
        catch (System.Exception e)
        {
            _logger.LogInformation("Company didn't crated");
            throw new Exception(e.Message);
        }
    }


    public async ValueTask<Result<Company>> CreateCompany(Company company, string userId, IFormFile file)
    {
        try
        {
            var filePath = "";
            if(file is null) {  filePath = Path.Combine("DefaultCompany.png");}
            
            else  {filePath = UploadCompanyPhoto(file);}
            
            if(company.Id != 0)
            {
              var existCopany = _unitOfWork.Companies.GetById(company.Id);

              if(existCopany is not null)
              {
                var updatedCompany = await _unitOfWork.Companies
                .Update(UpdatedCompany(existCopany, filePath, company));

                return new(true) {Data = ToModelCompany(updatedCompany)};
              }

            }
            if(company is null)
            return new("Company model can't be null");
            

            var createdCompany = await _unitOfWork.Companies.AddAsync(ToEntityCompany(company, filePath.ToString(), userId));

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

            if(contact.Id != 0)
            {
                var existContact = _unitOfWork.CompanyContacts.GetById(contact.Id);

                if(existContact is not null)
                {
                    var updatedContact = await _unitOfWork.CompanyContacts
                    .Update(ToEntityUpdateContact(existContact, contact));

                    return new(true) {Data = ToModelContact(updatedContact)};
                }
            }

            var createdContact = await _unitOfWork.CompanyContacts.AddAsync(ToEntityContact(contact, userId));

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

            
            if(companyLocation.Id != 0)
            {
               var existLocation = _unitOfWork.CompanyLocations.GetById(companyLocation.Id);

               if(existLocation is not null)
               {
                var updatedLocation = await _unitOfWork.CompanyLocations
                .Update(ToEntityUpdateLocation(existLocation, companyLocation));

                return new(true) {Data = ToModelCompanyLocation(updatedLocation)};
               }
            }

            var createdLocation = await _unitOfWork.CompanyLocations
            .AddAsync(ToEntityCompanyLocation(companyLocation));

            if(createdLocation is null)
            return new("Company Location is not created");

            System.Console.WriteLine(createdLocation.Description + "=====================>");
            
            return new(true){ Data = ToModelCompanyLocation(createdLocation)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Company Location is not created: {companyLocation}");
            throw new Exception(e.Message);
        }
    }

    public async void DeleteEmptyLocations(List<int> Ids, int companyId)
    {
        try
        {
            var locations = _unitOfWork.CompanyLocations.GetAll().Where(x => x.CompanyId == companyId).ToList();

            for (int i = 0; i < locations.Count(); i++)
            {
                bool exist = false;
                for (int j = 0; j < Ids.Count(); j++)
                {
                    if(locations[i].Id == Ids[j])
                    {
                        exist = true;
                    }
                }
                if(!exist)
                {
                  await  _unitOfWork.CompanyLocations.Remove(locations[i]);
                }
            }
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"not deleted locations with : {Ids}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<List<Company>>> GetAllCompany()
    {
       try
       {
         var existCopanies = _unitOfWork.Companies.GetAll();

         if(existCopanies is null)
         return new("Companies not found");

         return new(true) {Data = existCopanies.Select(x => ToModelCompany(x)).ToList()};
       }
       catch (System.Exception e) 
       {
        _logger.LogInformation($"Companies didn't taked");
        throw new Exception(e.Message);
       }
    }

    public async ValueTask<Result<Company>> GetCompanyById(int id)
    {
        try
        {
            if(id <= 0)
            return new("Id can't be thero ot Minus");

            var company = _unitOfWork.Companies.GetAll().Include(x => x.CompanyLocations).FirstOrDefault(x => x.Id == id);

            if(company is null)
            return new("This Company not Found");

            return new(true) {Data = ToModelCompany(company)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Company didn't taked with Id:{id}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<List<Company>>> GetCompanyWithPagination(int page, int limit)
    {
        try
        {
          if(page <= 0 || limit <= 0)
          return new("Page or limit is null or minus there");

          var existCopanies = _unitOfWork.Companies.GetAll();

          if(existCopanies is null)
          return new("Companies not found");

          var filteredCompanies = existCopanies
          .Skip((page-1)*limit)
          .Take(limit)
          .Select(x => ToModelCompany(x))
          .ToList();

          return new(true) {Data = filteredCompanies };
        }
        catch (System.Exception e)
        {
            _logger.LogInformation("Companies not found");
            throw new Exception(e.Message);
        }
    }

    public string UploadCompanyPhoto(IFormFile file)
    {
        try
        {
            if(file is null)
            return new("File Can't be null");

            var path = Path.Combine( file.FileName);
            var createdFile = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path), FileMode.Create);
            file.CopyTo(createdFile);

            createdFile.Close();
            
            return path;
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"File is not uploaded: {file.Name}");
            throw new Exception(e.Message);
        }
    }
}