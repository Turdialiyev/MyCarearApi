using System.Collections.Generic;
using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Services.Chat;

public interface IMessageService
{
    Message AddMessage(Message message);
    Task<Message> UpdateMessage(Message message);
    void RemoveMessage(int Id);
    IList<Entities.Chat> GetChats(string userId);
    IList GetChatsByUserInformations(string userId, IDictionary<string, List<string>> users);
    IList SearchUsers(string key, Dictionary<string, List<string>> users);
    string LocateFile(IFormFile file);
    Task<Message> ReadMessage(int id);
}
