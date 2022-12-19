using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IPortfolioService
{
    Result<FreelancerPortfolio> GetById(string userId);
    ValueTask<Result<FreelancerPortfolio>> SaveAsync(string userId, IFormFile image, FreelancerPortfolio portfolio);
    ValueTask<Result<FreelancerPortfolio>> UpdateAsync(string userId, string available);
}