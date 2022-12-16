using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
// using MyCarearApi.Migrations;
using MyCarearApi.Repositories;
using MyCarearApi.Repositories.Interfaces;
using System.Collections;

namespace MyCarearApi.Services.Chat;

public class MessageService: IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IFreelancerInformationRepository _freelancerInformationRepository;
    private readonly IChatRepository _chatRepository;
    private readonly UserManager<AppUser> _userManager;

    public MessageService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
    {
        _messageRepository = unitOfWork.Messages;
        _userManager = userManager;
        _companyRepository = unitOfWork.Companies;
        _freelancerInformationRepository = unitOfWork.FreelancerInformations;
        _chatRepository = unitOfWork.Chats;
    }

    public Message AddMessage(Message message)
    {
        message.DateTime = DateTime.Now;
        var chat = _chatRepository.Find(x => (x.Member1 == message.FromId && x.Member2 == message.ToId)
                                            || (x.Member2 == message.FromId && x.Member1 == message.ToId)).FirstOrDefault();
        if(chat is null)
        {
            chat = _chatRepository.Add(new Entities.Chat
            {
                DateTime = DateTime.Now,
                Member1 = message.FromId,
                Member2 = message.ToId,
            });
        }
        message.ChatId = chat.Id;
        return _messageRepository.Add(message);
    }

    public void RemoveMessage(int Id)
    {
        _messageRepository.Remove(new Message { Id = Id });
    }

    public async Task<Message> UpdateMessage(Message message) => await _messageRepository.Update(message);

    public IList<Entities.Chat> GetChats(string userId) =>
        _chatRepository.GetAll().Where(x => x.Member1 == userId || x.Member2 == userId).Include(x => x.Messages).ToList();

    public IList GetChatsByUserInformations(string userId, IDictionary<string, List<string>> users)=> GetChats(userId).Select(async x =>
        {
            var Member1 = await _userManager.FindByIdAsync(x.Member1);
            var Member2 = await _userManager.FindByIdAsync(x.Member2);
            return new
            {
                x.Id,
                x.DateTime,
                x.Messages,
                Member1 = new
                {
                    Member1.Id,
                    Member1.UserName,
                    Member1.FirstName,
                    Member1.LastName,
                    Member1.Email,
                    Member1.PhoneNumber,
                    IsOnline = users.ContainsKey(x.Member1) && users[x.Member1].Count > 0
                },
                Member2 = new
                {
                    Member2.Id,
                    Member2.UserName,
                    Member2.FirstName,
                    Member2.LastName,
                    Member2.Email,
                    Member2.PhoneNumber,
                    IsOnline = users.ContainsKey(x.Member2) && users[x.Member2].Count > 0
                }
            };
        }).Select(x => x.Result).ToList();


    private async Task<string> PhotoUrl(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        var photoPath = "no file";
        if(await _userManager.IsInRoleAsync(user, StaticRoles.Company))
        {
            var company = _companyRepository.GetAll().FirstOrDefault(x => x.AppUserId == userId);
            if (company is not null)
            {

                photoPath = company.Photo;
                Console.WriteLine(photoPath);
            }
            if (!File.Exists(photoPath))
            {
                photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DefaultUser.png");
            }
        }
        if(await _userManager.IsInRoleAsync(user, StaticRoles.Freelancer))
        {
            var freelancerInfo = _freelancerInformationRepository.GetAll().FirstOrDefault(x => x.AppUserId == userId);
            if (freelancerInfo is not null) photoPath = freelancerInfo.FreelancerImage;
            if (!File.Exists(photoPath))
            {
                photoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "DefaultCompany.png");
            }
        }
        Console.WriteLine("Path of   " + userId + "   " + photoPath);
        return photoPath;
    }

    public IList SearchUsers(string key, Dictionary<string, List<string>> users)
    {
        return _userManager.Users.Where(x => x.UserName.Contains(key) || x.FirstName.Contains(key) || x.LastName.Contains(key))
            .Select(x => new
            {
                x.Id,
                x.UserName,
                x.FirstName,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                IsOnline = users.ContainsKey(x.Id) && users[x.Id].Count > 0
            }).ToList();
    }

    public string LocateFile(IFormFile file)
    {//localhost:port/fielname.extension
        //wwwroot/D://
        string fileName = Path.Combine("MessageFiles", Guid.NewGuid().ToString() + file.FileName);
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",  fileName);
        file.CopyTo(new FileStream(path, FileMode.CreateNew));
        return fileName;
    }

    public async Task<Message> ReadMessage(int id)
    {
        var message = _messageRepository.GetById(id);
        message.IsRead = true;
        return await _messageRepository.Update(message);
    }
}


