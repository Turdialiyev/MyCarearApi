using MyCarearApi.Data;
using MyCarearApi.Repositories;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class JobRepository: GenericRepository<Job>, IJobRepository
    {
        public JobRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
