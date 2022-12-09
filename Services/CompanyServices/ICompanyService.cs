using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface ICompanyService
{
   ValueTask<Result<Company>> CreateCompany(Company company, string userId);//
   ValueTask<Result<string>> UploadCompanyPhoto(IFormFile file);//
   ValueTask<Result<Contact>> CreateCompanyContact(Contact contact, string userId);//
   ValueTask<Result<CompanyLocation>> CreateCompanyLocation(CompanyLocation companyLocation); 
   ValueTask<Result<AppUser>> CreateCopmanyUser(AppUser user, string userId);//

}