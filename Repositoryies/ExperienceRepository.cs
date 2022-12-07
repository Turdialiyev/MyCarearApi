using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class ExperienceRepository : GenericRepository<Experience>, IExperienceRepository
{
    public ExperienceRepository(AppDbContext context) : base(context)
    {
    }
}