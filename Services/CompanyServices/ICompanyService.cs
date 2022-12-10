using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface ICompanyService
{
   ValueTask<Result<Company>> CreateCompany(Company company, string userId, IFormFile file);//
   string UploadCompanyPhoto(IFormFile file);//
   ValueTask<Result<Contact>> CreateCompanyContact(Contact contact, string userId);//
   ValueTask<Result<CompanyLocation>> CreateCompanyLocation(CompanyLocation companyLocation);
   ValueTask<Result<AppUser>> CrateCompanyUser(AppUser user, string userId);//
   public void DeleteEmptyLocations(List<int> Ids, int companyId); 
    
}