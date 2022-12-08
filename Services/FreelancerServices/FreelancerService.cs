using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class FreelancerService : IFreelancerService
{
    private readonly ILogger<FreelancerService> _logger;
    private readonly IUnitOfWork _unitOfwork;
    private readonly IFileHelper _fileHelper;

    public FreelancerService(ILogger<FreelancerService> logger, IUnitOfWork unitOfwork, IFileHelper fileHelper)
    {
        _logger = logger;
        _unitOfwork = unitOfwork;
        _fileHelper = fileHelper;
    }

    public async ValueTask<Result<FreelancerInformation>> Information(int userId, FreelancerInformation information, IFormFile image)
    {
        string? filePath = null;
        try
        {
            if (information is null)
                return new("Null reference error");
            // UserId Tekshiriladi bu yerda bazada bormi yoki yo'q

            var freelancerInformation = _unitOfwork.FreelancerInformations.GetAll().FirstOrDefault(f => f.AppUserId == userId);

            if (freelancerInformation is null)
            {
                if (image is not null)
                {
                    try
                    {
                        if (!_fileHelper.FileValidateImage(image))
                            return new("File is invalid only picture");

                        filePath = await _fileHelper.WriteFileAsync(image, FileFolders.UserImage);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }
                freelancerInformation = await _unitOfwork.FreelancerInformations.AddAsync(ToEntity(information, filePath!, userId));
            }
            else
            {
                if (image is not null)
                {
                    try
                    {
                        if (!_fileHelper.FileValidateImage(image))
                            return new("File is invalid only picture");

                        if (freelancerInformation.FreelancerImage is not null)
                            _fileHelper.DeleteFileByName(freelancerInformation.FreelancerImage!);

                        filePath = await _fileHelper.WriteFileAsync(image, FileFolders.UserImage);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                }

                freelancerInformation.FirstName = information.FirstName;
                freelancerInformation.LastName = information.LastName;
                freelancerInformation.PhoneNumber = information.PhoneNumber;
                freelancerInformation.Email = information.Email;
                freelancerInformation.FreelancerImage = filePath;
                freelancerInformation.AppUserId = userId;

                freelancerInformation = await _unitOfwork.FreelancerInformations.Update(freelancerInformation);
            }

            return new(true) { Data = ToModel(freelancerInformation) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't Freelancer Information. Plaese contact support", e);
        }
    }

    private FreelancerInformation ToModel(Entities.FreelancerInformation freelancerInformation) => new()
    {
        FirstName = freelancerInformation.FirstName,
        LastName = freelancerInformation.LastName,
        Email = freelancerInformation.Email,
        PhoneNumber = freelancerInformation.PhoneNumber,
        FreelancerImage = freelancerInformation.FreelancerImage,
    };

    private Entities.FreelancerInformation ToEntity(FreelancerInformation information, string filePath, int userId) => new()
    {
        FirstName = information.FirstName,
        LastName = information.LastName,
        Email = information.Email,
        PhoneNumber = information.PhoneNumber,
        FreelancerImage = filePath,
        AppUserId = userId
    };

    public ValueTask<Result<Address>> Address(int freelancerId, Address address)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Contact>> Contact(int freelancerId, List<Contact> contacts)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Education>> Education(int freelancerId, List<Education> educations)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Position>> Position(int freelancerId, Position possion)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<UserLanguage>> UserLanguage(int freelancerId, List<UserLanguage> userLanguage)
    {
        throw new NotImplementedException();
    }

    public ValueTask<Result<Experience>> WorkExperience(int freelancerId, List<Experience> experiences)
    {
        throw new NotImplementedException();
    }
}