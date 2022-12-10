using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class JobLanguageRepository: GenericRepository<JobLanguage>, IJobLanguageRepository
    {
        public JobLanguageRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
