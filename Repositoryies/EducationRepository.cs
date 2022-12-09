using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class EducationRepository : GenericRepository<Education>, IEducationRepository
{
    public EducationRepository(AppDbContext context) : base(context) { }
}