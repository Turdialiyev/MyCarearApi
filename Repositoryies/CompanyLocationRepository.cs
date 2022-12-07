using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class CompanyLocationRepository : GenericRepository<CompanyLocation>, ICompanyLocationRepository
{
    public CompanyLocationRepository(AppDbContext context) : base(context)
    {
    }
}