using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerProjectRepository : GenericRepository<FreelancerProject>, IFreelancerProjectRepository
{
    public FreelancerProjectRepository(AppDbContext context) : base(context) { }
} 