using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IPortfolioService
{
    ValueTask<Result<FreelancerPortfolio>> SaveAsync(string userId, IFormFile image, FreelancerPortfolio portfolio);
    ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, IFormFile image, FreelancerPortfolio portfolio);
    ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, string available);
}