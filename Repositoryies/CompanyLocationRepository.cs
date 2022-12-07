using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class CompanyLocationRepository : GenericRepository<CompanyLocation>
{
    public CompanyLocationRepository(AppDbContext context) : base(context)
    {
    }
}