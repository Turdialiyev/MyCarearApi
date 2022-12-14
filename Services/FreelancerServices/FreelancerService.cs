using MyCarearApi.Models;
using MyCarearApi.Repositories;
using MyCareerApi.Entities;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using MyCarearApi.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace MyCarearApi.Services;

public partial class FreelancerService : IFreelancerService
{
    private readonly ILogger<FreelancerService> _logger;
    private readonly IUnitOfWork _unitOfwork;
    private readonly IFileHelper _fileHelper;
    private readonly UserManager<Entities.AppUser> _userManager;

    public FreelancerService(
        ILogger<FreelancerService> logger,
        IUnitOfWork unitOfwork,
        IFileHelper fileHelper,
        UserManager<Entities.AppUser> userManager)
    {
        _logger = logger;
        _unitOfwork = unitOfwork;
        _fileHelper = fileHelper;
        _userManager = userManager;
    }

    public async ValueTask<Result<FreelancerInformation>> Information(string userId, FreelancerInformation information, IFormFile image)
    {

        string? filePath = null;
        try
        {

            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            if (information is null)
                return new("User Information Null reference error");

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "userId not found" };

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
                if (File.Exists(freelancerInformation.FreelancerImage))
                    _fileHelper.DeleteFileByName(freelancerInformation.FreelancerImage!);

                try
                {
                    if (!_fileHelper.FileValidateImage(image))
                        return new("File is invalid only picture");

                    if (image is not null)
                        filePath = await _fileHelper.WriteFileAsync(image, FileFolders.UserImage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                freelancerInformation.FirstName = information.FirstName;
                freelancerInformation.LastName = information.LastName;
                freelancerInformation.PhoneNumber = information.PhoneNumber;
                freelancerInformation.Email = information.Email;
                freelancerInformation.FreelancerImage = filePath;
                freelancerInformation.AppUserId = userId;

                freelancerInformation = await _unitOfwork.FreelancerInformations.Update(freelancerInformation);
            }

            return new(true) { Data = ToModel(GetByIdFreelancer(freelancerInformation.Id).Result) };
        }
        catch (Exception e)
        {

            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't Freelancer Information. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<FreelancerInformation>> Address(string userId, Address address)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            if (address is null)
                return new("Adress Null reference error");

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "userId not found" };

            var existInformation = _unitOfwork.FreelancerInformations.GetAll().FirstOrDefault(f => f.AppUserId == userId);

            if (existInformation is null)
                return new(false) { ErrorMessage = "Freelancer information not found" };

            var existAddress = _unitOfwork.Addresses.GetAll().FirstOrDefault(a => a.FrelancerInformationId == existInformation.Id);

            if (existAddress is null)
            {
                existAddress = await _unitOfwork.Addresses.AddAsync(ToEntityAddress(existInformation.Id, address));
                existInformation.AddressId = existAddress.Id;
                await _unitOfwork.FreelancerInformations.Update(existInformation);

            }
            if (existAddress is not null)
            {
                existAddress.CountryId = address.CountryId;
                existAddress.RegionId = address.RegionId;
                existAddress.Home = address.Home;

                existAddress = await _unitOfwork.Addresses.Update(existAddress);
            }


            return new(true) { Data = ToModel(GetByIdFreelancer(existInformation.Id).Result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);
            throw new("Couldn't  update or create Adress. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<FreelancerInformation>> Position(int freelancerId, Position position)
    {
        var existFreelancer = _unitOfwork.FreelancerInformations.GetAll().Where(a => a.Id == freelancerId)
            .Include(x => x.Hobbies).Include(x => x.FreelancerSkills).FirstOrDefault();

        var skills = position.PositionSkills;
        var hobbies = position.FreelancerHobbies;
        var createdSkill = _unitOfwork.FreelancerSkills;

        if (existFreelancer is null)
            return new("Freelancer could not be found");

        if (position is null)
            return new("Posstion could not be been null");

        if (position.PositionSkills is null)
            return new("Skills should not be null");
        try
        {

            existFreelancer.Birthday = position.Birthday;
            existFreelancer.Description = position.Description;
            existFreelancer.PositionId = position.PositionId;

            await _unitOfwork.FreelancerInformations.Update(existFreelancer);

            var existSkills = _unitOfwork.FreelancerSkills.GetAll().Where(s => s.FreelancerInformationId == freelancerId);
            var existHobbies = _unitOfwork.FreelancerHobbies.GetAll().Where(h => h.FreelancerInformationId == freelancerId);



            var frelanceSkills = _unitOfwork.FreelancerSkills.GetAll().ToList();


            // bazadagi va kelgan skillarni solishtiradi va bazada yo'q skilni qaytaradi bazaga saqlash uchun
            var newSkills = skills!
                    .Where(freelancerSkill => !existSkills!
                    .Select(x => x.SkillId)
                    .Contains(freelancerSkill.SkillId)).ToList();

            // bazadagi va kelgan skillarni tekshiradi kelgan skill ichida yo'q sikillarni qaytaradi o'chirish uchun
            var skillIdsToDelete = existSkills!
                   .Where(skill => !skills!
                   .Select(x => x.SkillId)
                   .Contains(skill.SkillId)).ToList();

            var newHobbiess = hobbies!
                   .Where(freelancerHobby => !existHobbies!
                   .Select(y => y.HobbyId)
                   .Contains(freelancerHobby.HobbyId)).ToList();

            var hobbiesToDelete = existHobbies!
                    .Where(hobby => !hobbies!
                    .Select(y => y.HobbyId)
                    .Contains(hobby.HobbyId)).ToList();



            if (skillIdsToDelete is not null)
                await _unitOfwork.FreelancerSkills.RemoveRange(skillIdsToDelete);

            if (newSkills is not null)
                await _unitOfwork.FreelancerSkills.AddRange(newSkills.Select(s =>
                    new Entities.FreelancerSkill
                    {
                        SkillId = s.SkillId,
                        FreelancerInformationId = freelancerId
                    }));

            await _unitOfwork.FreelancerHobbies.AddRange(newHobbiess.Select(x =>
                new Entities.FreelancerHobby
                {
                    HobbyId = x.HobbyId,
                    FreelancerInformationId = freelancerId
                }));

            await _unitOfwork.FreelancerHobbies.RemoveRange(hobbiesToDelete);

            return new(true) { Data = ToModel(GetByIdFreelancer(existFreelancer.Id).Result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);
            throw new("Couldn't  update or create Postion. Plaese contact support", e);
        }
    }

    private Position ToModelPostion(Entities.FreelancerInformation existFreelancer) => new()
    {
        FreelancerId = existFreelancer.Id,
        PositionId = existFreelancer.PositionId,
        Birthday = existFreelancer.Birthday,
        Description = existFreelancer.Description,
        FreelancerHobbies = existFreelancer.Hobbies!.Select(x =>
            new FreelancerHobby
            {
                Id = x.Id,
                HobbyId = x.HobbyId,
            }),
        PositionSkills = existFreelancer.FreelancerSkills!.Select(x =>
            new FreelancerSkill
            {
                Id = x.Id,
                SkillId = x.SkillId,
            })
    };

    public async ValueTask<Result<FreelancerInformation>> Contact(int freelancerId, FreelancerContact contacts)
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

            if (contacts is not null)
            {
                if (existContact is null)
                {
                    existContact = await _unitOfwork.FreelancerContacts.AddAsync(ToEntityContact(contacts, freelancerId));
                }

                if (existContact is not null)
                {

                    existContact.WebSite = contacts.WebSite;
                    existContact.Facebook = contacts.Facebook;
                    existContact.Instagram = contacts.Instagram;
                    existContact.Telegram = contacts.Telegram;
                    existContact.GitHub = contacts.GitHub;
                    existContact.Twitter = contacts.Twitter;
                    existContact.WatsApp = contacts.WatsApp;

                    existContact = await _unitOfwork.FreelancerContacts.Update(existContact);
                }
            }
            if (contacts is null && existContact is not null)
            {
                existContact = await _unitOfwork.FreelancerContacts.Remove(existContact);
            }

            return new(true) { Data = ToModel(GetByIdFreelancer(exsitFreelancer.Id).Result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't create Contact. Plaese contact support", e);
        }

    }

    public async ValueTask<Result<FreelancerInformation>> Resume(int freelancerId, TypeResume resume)
    {
        if (freelancerId == 0)
            return new("FreelancerId is invalid");

        var exsitFreelancer = _unitOfwork.FreelancerInformations.GetById(freelancerId);
        try
        {

            if (exsitFreelancer is null)
                return new("Freelancer is not found");

            exsitFreelancer.TypeResume = resume;
            await _unitOfwork.FreelancerInformations.Update(exsitFreelancer);

            return new(true) { Data = ToModel(GetByIdFreelancer(exsitFreelancer.Id).Result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't create Resume Type . Plaese contact support", e);
        }
    }

    public async ValueTask<Result<FreelancerInformation>> FinishResume(int freelancerId, bool finish)
    {
        if (freelancerId == 0)
            return new("FreelancerId is invalid");
        try
        {
            var exsitFreelancer = _unitOfwork.FreelancerInformations.GetById(freelancerId);

            if (exsitFreelancer is null)
                return new("Freelancer is not found");

            exsitFreelancer.Finish = finish;
            await _unitOfwork.FreelancerInformations.Update(exsitFreelancer);

            return new(true) { Data = ToModel(GetByIdFreelancer(exsitFreelancer.Id).Result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)}", e);

            throw new("Couldn't update Resuume Finish. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<List<FreelancerInformation>>> GetAll()
    {
        try
        {
            var freelancers = await _unitOfwork.FreelancerInformations.GetAll().Where(x => x.Finish == true)
                            .Include(x => x.UserLanguages)!.ThenInclude(x => x.Language)
                            .Include(x => x.Position)
                            .Include(x => x.FreelancerSkills)!.ThenInclude(x => x.Skill)
                            .Include(x => x.Hobbies)!.ThenInclude(x => x.Hobby)
                            .Include(x => x.Address)
                            .Include(x => x.Address)!.ThenInclude(c => c!.Country)
                            .Include(x => x.Address)!.ThenInclude(r => r!.Region)
                            .Include(x => x.Experiences)
                            .Include(x => x.Educations)
                            .Include(x => x.FreelancerContact)
                            .ToListAsync();

            if (!freelancers.Any())
                return new(false) { ErrorMessage = "Any freelancers not found" };

            return new(true) { Data = freelancers.Select(ToModel).ToList() };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)} .", e);
            throw new("Couldn't get Freelancers GetAll Please contact support");
        }
    }

    private async ValueTask<Entities.FreelancerInformation> GetByIdFreelancer(int freelancerId)
    {
        var freelancer = await _unitOfwork.FreelancerInformations.GetAll().Where(x => x.Id == freelancerId)
                                .Include(x => x.UserLanguages)!.ThenInclude(x => x.Language)
                                .Include(x => x.Position)
                                .Include(x => x.FreelancerSkills)!.ThenInclude(x => x.Skill)
                                .Include(x => x.Hobbies)!.ThenInclude(x => x.Hobby)
                                .Include(x => x.Address)
                                .Include(x => x.Address)!.ThenInclude(c => c!.Country)
                                .Include(x => x.Address)!.ThenInclude(r => r!.Region)
                                .Include(x => x.Experiences)
                                .Include(x => x.Educations)
                                .Include(x => x.FreelancerContact)
                                .FirstOrDefaultAsync();

        return freelancer!;
    }

    public async ValueTask<Result<FreelancerInformation>> GetById(int freelancerId)
    {
        try
        {
            var freelancer = _unitOfwork.FreelancerInformations.GetById(freelancerId);

            if (freelancer == null)
                return new(false) { ErrorMessage = "Freelancer not found" };

            return new(true) { Data = ToModel(GetByIdFreelancer(freelancerId).Result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(FreelancerService)} .", e);
            throw new("Couldn't get Freelancers GetAll Please contact support");
        }
    }

}