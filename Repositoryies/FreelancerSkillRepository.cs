using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerSkillRepository : GenericRepository<FreelancerSkill>, IFreelancerSkillRepository
{
    public FreelancerSkillRepository(AppDbContext context) : base(context) { }
}