using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerContactRepository : GenericRepository<FreelancerContact>, IFreelancerContactRepository
{
    public FreelancerContactRepository(AppDbContext context) : base(context) { }
}