# pragma warning disable
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
    public async ValueTask<Result<FreelancerProject>> DeleteAsync(int id)
    {
        var projectPath = _fileHelper.Folder(FileFolders.Project);
        var imagePath = _fileHelper.Folder(FileFolders.ProjectImages);
        try
        {
            var existProject = _unitOfWork.FreelancerProjects.GetById(id);

            if (existProject == null)
                return new(false) { ErrorMessage = "project id is not found" };

            var existProjectImages = _unitOfWork.ProjectImages.GetAll().Where(x => x.FreelancerProjectId == existProject.Id);

            if (existProjectImages.Any())
            {
                foreach (var item in existProjectImages)
                {
                    if (File.Exists(imagePath + @"\" + item.Name))
                    {
                        _fileHelper.DeleteFileByName(imagePath, item.Name!);
                    }

                    await _unitOfWork.ProjectImages.Remove(item);
                }
            }

            if (File.Exists(projectPath + @"\" + existProject.Project))
                _fileHelper.DeleteFileByName(projectPath, existProject.Project!);

            existProject = await _unitOfWork.FreelancerProjects.Remove(existProject);

            return new(true) { Data = ToModel(existProject) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
            throw new("Couldn't project delete Please contact support");
        }
    }


    public async ValueTask<Result<FreelancerProject>> SaveAsync(string userId, IFormFile projectFile, IFormFileCollection projectFiles, FreelancerProject project)
    {
        var check = projectFiles == null ? 0 : projectFiles.Count();
        string? file = null;
        string? image = null;
        var id = 0;
        var fileFolder = FileFolders.Project;
        var projectImageFolder = FileFolders.ProjectImages;

        try
        {
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

            if (check > 0)
            {
                for (int i = 0; i < projectFiles!.Count(); i++)
                {
                    if (projectFiles[i]! is not null)
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

            var result = _unitOfWork.FreelancerProjects.GetAll().Include(x => x.ProjectImages).FirstOrDefault(x => x.Id == id);

            return new(true) { Data = ToModel(createdProject!) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
            throw new("Couldn't get project create Please contact support");
        }
    }

    public async ValueTask<Result<FreelancerProject>> UpdateAsync(int id, IFormFile projectFile, IFormFileCollection projectFiles, FreelancerProject project, int[] deleteId)
    {

        var check = projectFiles == null ? 0 : projectFiles.Count();
        var delete = deleteId == null ? 0 : deleteId.Count();
        string? file = null;
        string? image = null;
        var fileFolder = FileFolders.Project;
        var projectImageFolder = FileFolders.ProjectImages;
        var fullPathToProject = _fileHelper.Folder(fileFolder);
        var fullPathImages = _fileHelper.Folder(projectImageFolder);

        try
        {
            if (id <= 0)
                return new(false) { ErrorMessage = "User Id invalid" };

            var existProject = _unitOfWork.FreelancerProjects.GetAll().Include(x => x.ProjectImages).FirstOrDefault(x => x.Id == id);

            if (existProject is null)
                return new(false) { ErrorMessage = "project Id is not found " };

            if (project is null)
                return new(false) { ErrorMessage = "project null exseption " };

            if (delete > 0)
            {
                foreach (var item in deleteId)
                {
                    var existProjectImage = _unitOfWork.ProjectImages.GetById(item);

                    if (existProjectImage is null)
                        return new(false) { ErrorMessage = $"Image Is {item} not found" };

                    if (File.Exists(fullPathImages + @"\" + existProjectImage.Name))
                        _fileHelper.DeleteFileByName(fullPathImages, existProjectImage.Name);

                    await _unitOfWork.ProjectImages.Remove(existProjectImage);
                }
            }

            if (check > 0)
            {
                for (int i = 0; i < projectFiles!.Count(); i++)
                {
                    if (projectFiles[i]! is not null)
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

                    var createdProjectFile = await _unitOfWork.ProjectImages.AddAsync(ToEntityImage(existProject.Id, image));
                    image = null;
                }
            }

            if (projectFile is not null)
            {
                try
                {
                    if (!_fileHelper.FileValidate(projectFile))
                        return new(false) { ErrorMessage = "Project file is invalid" };

                    if (File.Exists(fullPathToProject + @"\" + existProject.Project))
                        _fileHelper.DeleteFileByName(fullPathToProject, existProject.Project);

                    file = await _fileHelper.WriteFileAsync(projectFile, fileFolder);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            if (projectFile is null)
            {
                if (File.Exists(fullPathToProject + @"\" + existProject.Project))
                    _fileHelper.DeleteFileByName(fullPathToProject, existProject.Project);
            }

            existProject.Project = file;
            existProject.Title = project.Title;
            existProject.Tools = project.Tools;
            existProject.Description = project.Description;
            existProject.Link = project.Link;

            return new(true) { Data = ToModel(existProject) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(ProjectService)} .", e);
            throw new("Couldn't project Update Please contact support");
        }
    }

    private FreelancerProject ToModel(Entities.FreelancerProject createdProject) => new()
    {
        Id = createdProject.Id,
        Description = createdProject.Description,
        Title = createdProject.Title,
        Tools = createdProject.Tools,
        Link = createdProject.Link,
        Project = createdProject.Project,
        ProjectImages = createdProject.ProjectImages!.Select(x => new ProjectImage()
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

}