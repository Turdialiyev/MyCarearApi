using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class FreelancerPortfolioRepository : GenericRepository<FreelancerPortfolio>, IFreelancerPortfolioRepository
{
    public FreelancerPortfolioRepository(AppDbContext context) : base(context) { }
}