using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class CompanyContactRepository : GenericRepository<CompanyContact>, ICompanyContactRepository
{
    public CompanyContactRepository(AppDbContext context) : base(context) { }
}