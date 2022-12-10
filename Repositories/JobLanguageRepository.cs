using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;

namespace MyCarearApi.Repositoryies
{
    public class JobLanguageRepository: GenericRepository<JobLanguage>, IJobLanguageRepository
    {
        public JobLanguageRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
