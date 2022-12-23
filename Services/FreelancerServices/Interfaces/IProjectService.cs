using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IProjectService
{
    Result<FreelancerProject> GetById(int id);
    ValueTask<Result<FreelancerProject>> SaveAsync(string userId, IFormFile projectFile, IFormFileCollection projectfiles, FreelancerProject project);
    ValueTask<Result<FreelancerProject>> UpdateAsync(int id, IFormFile projectFile, IFormFileCollection projectFiles, FreelancerProject project, int[] deleteId);
    ValueTask<Result<FreelancerProject>> DeleteAsync(int id);
}