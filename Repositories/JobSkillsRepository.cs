using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class JobSkillsRepository: GenericRepository<JobSkill>, IJobSkillsRepository
    {
        public JobSkillsRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
