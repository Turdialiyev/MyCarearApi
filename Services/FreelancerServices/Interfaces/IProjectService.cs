using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IProjectService
{
    Result<FreelancerProject> GetById(int id);
    ValueTask<Result<FreelancerProject>> SaveAsync(string userId, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project);
    ValueTask<Result<FreelancerProject>> UpdateAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project);
    ValueTask<Result<FreelancerProject>> DeleteAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project);
}