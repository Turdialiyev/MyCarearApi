using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerHobbyRepository : GenericRepository<FreelancerHobby>, IFreelancerHobbyRepository
{
    public FreelancerHobbyRepository(AppDbContext context) : base(context) { }
}