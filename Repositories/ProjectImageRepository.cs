using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class ProjectImageRepository : GenericRepository<ProjectImage>, IProjectImageRepository
{
    public ProjectImageRepository(AppDbContext context) : base(context) { }
}