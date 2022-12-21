using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class ProjectService : IProjectService
{
    private readonly ILogger<ProjectService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.AppUser> _userManager;
    private readonly IFileHelper _fileHelper;

    public ProjectService(
        ILogger<ProjectService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager,
        IFileHelper fileHelper)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _fileHelper = fileHelper;
    }
    public Result<FreelancerProject> GetById(int id)
    {
        try
        {
            var freelancerProject = _unitOfWork.FreelancerProjects.GetById(id);

            if (freelancerProject == null)
                return new(false) { ErrorMessage = "project is not found" };

            return new(true) { Data = ToModel(freelancerProject) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
            throw new("Couldn't get project GetbyId Please contact support");
        }
    }
    public async ValueTask<Result<FreelancerProject>> DeleteAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        try
        {
            var existProject = _unitOfWork.FreelancerProjects.GetById(id);

            if (existProject == null)
                return new(false) { ErrorMessage = "project id is not found" };

            existProject = await _unitOfWork.FreelancerProjects.Remove(existProject);

            return new(true) { Data = ToModel(existProject) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
            throw new("Couldn't get project GetbyId Please contact support");
        }
    }


    public async ValueTask<Result<FreelancerProject>> SaveAsync(string userId, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        string? file = null;
        string? image = null;
        var id = 0;
        var fileFolder = FileFolders.Project;
        var projectImageFolder = FileFolders.ProjectImages;

        // try
        // {
            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "User is not found " };

            if (project is null)
                return new(false) { ErrorMessage = "project null exseption" };

            if (projectFile is not null)
            {
                try
                {
                    if (!_fileHelper.FileValidate(projectFile))
                        return new(false) { ErrorMessage = "Project file is invalid" };

                    file = await _fileHelper.WriteFileAsync(projectFile, fileFolder);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }

            var createdProject = await _unitOfWork.FreelancerProjects.AddAsync(ToEntity(userId, file, project));

            if (createdProject is null)
                return new(false) { ErrorMessage = "project is not created " };

            id = createdProject.Id;

            if (projectFiles.Any())
            {
                for (int i = 0; i < projectFiles.Count(); i++)
                {
                    if (projectFiles[i] is not null)
                    {
                        try
                        {
                            if (!_fileHelper.FileValidateImage(projectFiles[i]))
                                return new(false) { ErrorMessage = "project Image file is invalid" };

                            image = await _fileHelper.WriteFileAsync(projectFiles[i], projectImageFolder);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                    }

                    var createdProjectFile = await _unitOfWork.ProjectImages.AddAsync(ToEntityImage(id, image));
                    image = null;
                }
            }
            var result =  _unitOfWork.FreelancerProjects.GetAll().Include(x => x.ProjectImages).FirstOrDefault(x => x.Id == id);

            return new(true) { Data = ToModel(result!)};
        // }
        // catch (Exception e)
        // {
        //     _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
        //     throw new("Couldn't get project create Please contact support");
        // }
    }

    private FreelancerProject ToModel(Entities.FreelancerProject createdProject) => new()
    {
        Id = createdProject.Id,
        Description = createdProject.Description,
        Title = createdProject.Title,
        Tools = createdProject.Tools,
        Link = createdProject.Link,
        Project = createdProject.Project,
        ProjectImage = createdProject.ProjectImages!.Select(x => new ProjectImage()
        {
            Id = x.Id,
            Name = x.Name
        })
    };

    private Entities.ProjectImage ToEntityImage(int id, string? image) => new()
    {
        Name = image,
        FreelancerProjectId = id,
    };

    private Entities.FreelancerProject ToEntity(string userId, string? file, FreelancerProject project) => new()
    {
        Title = project.Title,
        Description = project.Description,
        Tools = project.Tools,
        Link = project.Link,
        Project = file,
        AppUserId = userId
    };

    public ValueTask<Result<FreelancerProject>> UpdateAsync(int id, IFormFile projectFile, List<IFormFile> projectFiles, FreelancerProject project)
    {
        throw new NotImplementedException();
    }
}