using MyCarearApi.Repositoryies.Interfaces;
using MyCarearApi.Data;
using MyCarearApi.Repositories;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositoryies
{
    public class JobSkillsRepository: GenericRepository<JobSkills>, IJobSkillsRepository
    {
        public JobSkillsRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
