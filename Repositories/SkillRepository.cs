using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class SkillRepository : GenericRepository<Skill>, ISkillRepository
{
    public SkillRepository(AppDbContext context) : base(context) { }
}