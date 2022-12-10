using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerInformationRepository : GenericRepository<FreelancerInformation>, IFreelancerInformationRepository
{
    public FreelancerInformationRepository(AppDbContext context) : base(context) { }
}