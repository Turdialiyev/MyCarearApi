using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class CompanyContactRepository : GenericRepository<Contact>, ICompanyContactRepository
{
    public CompanyContactRepository(AppDbContext context) : base(context) { }
}