using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarrearApi.Data;
using MyCarrearApi.Repositories;

namespace MyCarearApi.Repositoryies
{
    public class JobRepository: GenericRepository<Job>, IJobRepository
    {
        public JobRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
