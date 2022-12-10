using MyCarearApi.Models;
using MyCarearApi.Repositories;
using MyCareerApi.Entities;

namespace MyCarearApi.Services;

public partial class FreelancerService : IFreelancerService
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

    public async ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image)
    {
        
        string? filePath = null;
        // fake userId Null qilindi kegin haqiqiy user olinadi
        userId = null;
        try
        {
            if (information is null)
                return new("Null reference error");
            // UserId Tekshiriladi bu yerda bazada bormi yoki yo'q
            
            // fake uchun tekshirilib ko'rildi
            var freelancerInformation = _unitOfwork.FreelancerInformations.GetAll().FirstOrDefault(f => f.Id == 1);

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

    public async ValueTask<Result<Address>> Address(int freelancerId, Address address)
    {
        var existInformation = _unitOfwork.FreelancerInformations.GetById(freelancerId);

        if (existInformation is null)
            return new("Freelancer Information not included");

        if (address is null)
            return new("Null reference error");

        try
        {
            var existAddress = _unitOfwork.Addresses.GetAll().FirstOrDefault(a => a.FrelancerInformationId == existInformation.Id);

            if (existAddress is null)
                existAddress = await _unitOfwork.Addresses.AddAsync(ToEntityAddress(freelancerId, address));

            if (existAddress is not null)
            {
                existAddress.CountryId = address.CountryId;
                existAddress.RegionId = address.RegionId;
                existAddress.Home = address.Home;

                existAddress = await _unitOfwork.Addresses.Update(existAddress);
            }

            return new(true) { Data = ToModelAdress(existAddress) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);
            throw new("Couldn't  update or create Adress. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<FreelancerInformation>> Position(int freelancerId, Position position)
    {
        var existFreelancer = _unitOfwork.FreelancerInformations.GetById(freelancerId);
        var skills = position.PositionSkills;
        var hobbies = position.Hobbies;
        var createdSkill = _unitOfwork.FreelancerSkills;
        if (existFreelancer is null)
            return new("Freelancer could not be found");

        if (position is null)
            return new("Posstion could not be been null");

        if (position.PositionSkills is null)
            return new("Skills should not be null");

        existFreelancer.Birthday = position.Birthday;
        existFreelancer.Description = position.Description;
        existFreelancer.PositionId = position.PositionId;
        await _unitOfwork.FreelancerInformations.Update(existFreelancer);

        var existSkills = _unitOfwork.FreelancerSkills.GetAll().Where(s => s.FrelanceInformationId == freelancerId);
        var existHobbies = _unitOfwork.FreelancerHobbies.GetAll().Where(h => h.FreelanceInformationId == freelancerId);

        if (existSkills is null)
        {
            await _unitOfwork.FreelancerSkills.AddRange(skills!.Select(x =>
                          new Entities.FreelancerSkill
                          {
                              SkillId = x,
                              FrelanceInformationId = freelancerId
                          }));
        }

        if (existHobbies is null && position.Hobbies is not null)
        {
            await _unitOfwork.FreelancerHobbies.AddRange(hobbies!.Select(y =>
                new Entities.FreelancerHobby
                {
                    HobbyId = y,
                    FreelanceInformationId = freelancerId
                }));
        }

        if (existSkills is not null)
        {

            //bor edi [1,3,4,6,4]
            //keldi [1,2,4,5,6]
            var newSkills = skills!
                    .Where(id => !existSkills!
                    .Select(x => x.Id)
                    .Contains(id));//[2,5]

            var skillIdsToDelete = existSkills!.Where(id => skills!.Contains(id.Id));

            if (skillIdsToDelete is not null)
                await _unitOfwork.FreelancerSkills.RemoveRange(skillIdsToDelete);

            if (newSkills is not null)
                await _unitOfwork.FreelancerSkills.AddRange(newSkills.Select(s =>
                    new Entities.FreelancerSkill
                    {
                        SkillId = s,
                        FrelanceInformationId = freelancerId
                    }));
        }

        if (existHobbies is not null)
        {
            var newHobbiess = hobbies!
                   .Where(id => !existHobbies!
                   .Select(x => x.Id)
                   .Contains(id));

            var hobbiesToDelete = existHobbies!
                    .Where(id => hobbies!
                    .Contains(id.Id));


            if (newHobbiess is not null)
                await _unitOfwork.FreelancerHobbies.AddRange(newHobbiess.Select(x =>
                    new Entities.FreelancerHobby
                    {
                        HobbyId = x,
                        FreelanceInformationId = freelancerId
                    }));

            if (hobbiesToDelete is not null)
                await _unitOfwork.FreelancerHobbies.RemoveRange(hobbiesToDelete);

        }

        return new(true) { Data = ToModelExistFreelancer(existFreelancer) };
    }

    public async ValueTask<Result<FreelancerContact>> Contact(int freelancerId, FreelancerContact contacts)
    {
        if (freelancerId == 0)
            return new("FreelancerId is invalid");

        if (contacts is null)
            return new(false) { ErrorMessage = "Contact is null" };

        try
        {
            var exsitFreelancer = _unitOfwork.FreelancerInformations.GetById(freelancerId);

            if (exsitFreelancer is null)
                return new("Freelancer is not found");

            var existContact = _unitOfwork.FreelancerContacts.GetAll().Where(c => c.FreelancerInformationId == exsitFreelancer.Id).FirstOrDefault();

            if (existContact is null)
            {
                existContact = await _unitOfwork.FreelancerContacts.AddAsync(ToEntityContact(contacts, freelancerId));
            }
            if (existContact is not null)
            {
                existContact.Facebook = contacts.Facebook;
                existContact.Instagram = contacts.Instagram;
                existContact.Telegram = contacts.Telegram;
                existContact.GitHub = contacts.GitHub;
                existContact.Twitter = contacts.Twitter;
                existContact.WatsApp = contacts.WatsApp;

                existContact = await _unitOfwork.FreelancerContacts.Update(existContact);
            }

            return new(true) { Data = ToModelContact(existContact) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't create Contact. Plaese contact support", e);
        }

    }

}