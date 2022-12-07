using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class PositionSkillRepository : GenericRepository<PositionSkill>, IPositionSkillRepository
{
    public PositionSkillRepository(AppDbContext context) : base(context) { }
}