using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
// using MyCarearApi.Migrations;
using MyCarearApi.Repositories;
using MyCarearApi.Repositories.Interfaces;
using System.Collections;
using System.Net.Mime;
#pragma warning disable
namespace MyCarearApi.Services.Chat;

public class MessageService: IMessageService
{
    private readonly IMessageRepository _messageRepository;
    private readonly ICompanyRepository _companyRepository;
    private readonly IFreelancerInformationRepository _freelancerInformationRepository;
    private readonly IChatRepository _chatRepository;
    private IChatFileRepository _chatFileRepository;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger logger;

    public MessageService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ILogger<MessageService> logger)
    {
        _messageRepository = unitOfWork.Messages;
        _userManager = userManager;
        _companyRepository = unitOfWork.Companies;
        _freelancerInformationRepository = unitOfWork.FreelancerInformations;
        _chatRepository = unitOfWork.Chats;
        _chatFileRepository = unitOfWork.ChatFiles;
        this.logger = logger;
    }

    public Message GetMessage(int id) => _messageRepository.GetAll().Include(x => x.Chat).Include(x => x.ChatFiles).FirstOrDefault(x => x.Id == id);

    public Message AddMessage(Message message, List<string> filePaths)
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
        var messageToReturn = _messageRepository.Add(message);
        SaveFiles(messageToReturn.Id, filePaths);
        return _messageRepository.GetAll().Include(x => x.Chat).Include(x=> x.ChatFiles).First(x => x.Id == messageToReturn.Id);
    }

    public void RemoveMessage(int id)
    {
        var chatFiles = _chatFileRepository.GetAll().Where(x => x.MessageId == id).ToList();
        if(chatFiles is not null && chatFiles.Count() > 0)
        {
            foreach (var file in chatFiles)
            {
                try
                {
                    File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.Path));
                    
                }
                catch(DirectoryNotFoundException e) { logger.LogError(e.Message, e.Data); }
                catch (NotSupportedException e) { logger.LogError(e.Message, e.Data); }
                catch (PathTooLongException e) { logger.LogError(e.Message, e.Data); }
                catch(UnauthorizedAccessException e) { logger.LogError( e.Message, e.Data); }
                catch (IOException e) { logger.LogError(e.Message, e.Data); }
                finally { _chatFileRepository.Remove(file); }
            }
        }

        _messageRepository.Remove(new Message { Id = id });
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
                /*Messages = x.Messages
                    .Select(m => new Message 
                    { 
                        Id = m.Id, 
                        ChatId = m.ChatId,
                        DateTime = m.DateTime, 
                        FromId = m.FromId, 
                        IsRead = m.IsRead,
                        Text = m.Text, 
                        ToId = m.ToId,
                        ChatFiles = m.ChatFiles, 
                        HasFile = m.HasFile, 
                        HasMedia = m.HasMedia,
                        HasLink = m.HasLink
                }),*/
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
            if (!File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photoPath)))
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
                photoPath = "DefaultCompany.png";
            }
        }
        Console.WriteLine("Path of   " + userId + "   " + photoPath);
        return photoPath;
    }

    public IList SearchUsers(string key, Dictionary<string, List<string>> users, string currentUserId)
    {
        return _userManager.Users.Where(x => x.UserName.Contains(key) || x.FirstName.Contains(key) || x.LastName.Contains(key) 
            || x.Email.Contains(key) || x.PhoneNumber.Contains(key))
            .Select(x => new
            {
                x.Id,
                x.UserName,
                x.FirstName,
                x.LastName,
                x.Email,
                x.PhoneNumber,
                Photo = PhotoUrl(x.Id),
                IsOnline = users.ContainsKey(x.Id) && users[x.Id].Count > 0,
                HasChat = HasChat(currentUserId, x.Id)
            }).ToList();
    }

    private bool HasChat(string firstUserId, string secondUserId)=> _chatRepository.GetAll().Where(x => (x.Member1 == firstUserId && x.Member2 == secondUserId) ||
                                            (x.Member2 == firstUserId && x.Member1 == secondUserId)).Any();

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
        await _messageRepository.Update(message);
        return await Task.FromResult(_messageRepository.GetAll()
                                                       .Where(x => x.Id == id)
                                                       .Include(x => x.ChatFiles)
                                                       .Include(x => x.Chat).FirstOrDefault());
    }

    public Entities.Chat GetChat(int id) => _chatRepository.GetById(id);

    public async Task<dynamic> GetChat(int chatId, Dictionary<string, List<string>> users)
    {
        var chat = _chatRepository.GetAll().Include(x => x.Messages).ThenInclude(x => x.ChatFiles).FirstOrDefault(x => x.Id == chatId);
        var Member1 = await _userManager.FindByIdAsync(chat.Member1);
        var Member2 = await _userManager.FindByIdAsync(chat.Member2);
        return new
        {
            chat.Id,
            chat.DateTime,
            Messages = chat.Messages.Select(m => new Message
            {
                Id = m.Id,
                ChatId = m.ChatId,
                DateTime = m.DateTime,
                FromId = m.FromId,
                IsRead = m.IsRead,
                Text = m.Text,
                ToId = m.ToId,
                HasFile = m.HasFile,
                HasLink = m.HasLink,
                HasMedia = m.HasMedia,
                ChatFiles = m.ChatFiles
            }),
            Member1 = new
            {
                Member1.Id,
                Member1.UserName,
                Member1.FirstName,
                Member1.LastName,
                Member1.Email,
                Member1.PhoneNumber,
                IsOnline = users.ContainsKey(chat.Member1) && users[chat.Member1].Count > 0
            },
            Member2 = new
            {
                Member2.Id,
                Member2.UserName,
                Member2.FirstName,
                Member2.LastName,
                Member2.Email,
                Member2.PhoneNumber,
                IsOnline = users.ContainsKey(chat.Member2) && users[chat.Member2].Count > 0
            }
        };
    }

    public List<ChatFile> SaveFiles(int messageId, List<string> files)
    {
        var chatFiles = new List<ChatFile>();
        foreach (var file in files)
        {
            var fileExtension = file.Substring(file.LastIndexOf('.') + 1);
            Entities.Enums.MediaType mediaType = Entities.Enums.MediaType.Unknown;
            switch (fileExtension.ToLower())
            {
                case "pdf":
                case "json": 
                case "xml":
                case "zip":
                case "rar":
                    mediaType = Entities.Enums.MediaType.File; break;
                case "tif":
                case "jpg":
                case "jpeg":
                case "png":
                case "bmp":
                    mediaType = Entities.Enums.MediaType.Img; break;
                case "mp4":
                case "vob":
                case "mkv":
                case "flv":
                case "gif":
                case "avi":
                case "mpg":
                case "mpeg":
                    mediaType = Entities.Enums.MediaType.Video; break;
                case "mp3":
                    mediaType = Entities.Enums.MediaType.Voice;break; 
            }
            chatFiles.Add(_chatFileRepository.Add(new ChatFile { Path= file, MessageId = messageId, MediType = mediaType }));
        }
        return chatFiles;
    }

    public void ClearHistory(int chatId)
    {
        var chat = _chatRepository.GetAll().Include(x => x.Messages).First();
        chat.Messages.ForEach(x => RemoveMessage(x.Id));
    }

    public async Task RemoveFiles(List<int> fileIds)
    {
        foreach (int id in fileIds)
        {
            var file = _chatFileRepository.GetById(id);
            try
            {
                File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", file.Path));
            }
            catch (DirectoryNotFoundException e) { logger.LogError(e.Message, e.Data); }
            catch (NotSupportedException e) { logger.LogError(e.Message, e.Data); }
            catch (PathTooLongException e) { logger.LogError(e.Message, e.Data); }
            catch (UnauthorizedAccessException e) { logger.LogError(e.Message, e.Data); }
            catch (IOException e) { logger.LogError(e.Message, e.Data); }
            finally { await _chatFileRepository.Remove(file); }
        }
    }

    public async Task<Message> UpdateText(string text, int messageId)
    {
        var message = _messageRepository.GetById(messageId);
        message.Text = text;
        return await _messageRepository.Update(message);
    }
}


