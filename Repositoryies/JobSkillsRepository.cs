using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarrearApi.Data;
using MyCarrearApi.Repositories;

namespace MyCarearApi.Repositoryies
{
    public class JobSkillsRepository: GenericRepository<JobSkills>, IJobSkillsRepository
    {
        public JobSkillsRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
