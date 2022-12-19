using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IPortfolioService
{
    ValueTask<Result<FreelancerPortfolio>> SaveAsync(int userId, IFormFile image, FreelancerPortfolio portfolio);
    ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, IFormFile image, FreelancerPortfolio portfolio);
    ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, string available);
}