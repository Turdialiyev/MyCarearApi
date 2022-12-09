using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface ICompanyService
{
   ValueTask<Result<Company>> CreateCompany(Company company);
   ValueTask<Result<string>> UploadCompanyPhoto(IFormFile file);
   ValueTask<Result<Contact>> CreateCompanyContact(Contact contact);
   ValueTask<Result<CompanyLocation>> CreateCompanyLocation(CompanyLocation companyLocation);

}