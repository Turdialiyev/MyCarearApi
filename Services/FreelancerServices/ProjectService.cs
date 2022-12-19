using MyCarearApi.Models;

namespace MyCarearApi.Services;

public class Projectservice : IProjectService
{
    public ValueTask<Result<FreelancerProject>> DeleteAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<FreelancerProject>> SaveAsync(int userId, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<FreelancerProject>> UpdateAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        throw new NotImplementedException();
    }
}