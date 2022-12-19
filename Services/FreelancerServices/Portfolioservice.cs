using MyCarearApi.Models;

namespace MyCarearApi.Services;

public class PortfolioService : IPortfolioService
{
    public ValueTask<Result<FreelancerPortfolio>> SaveAsync(int userId, IFormFile image, FreelancerPortfolio portfolio)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, IFormFile image, FreelancerPortfolio portfolio)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, string available)
    {
        throw new NotImplementedException();
    }
}