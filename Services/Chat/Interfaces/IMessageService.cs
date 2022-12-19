using System.Collections.Generic;
using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Services.Chat;

public interface IMessageService
{
    Message GetMessage(int id);
    Message AddMessage(Message message, List<string> filePaths);
    Task<Message> UpdateMessage(Message message);
    void RemoveMessage(int Id);
    IList<Entities.Chat> GetChats(string userId);
    IList GetChatsByUserInformations(string userId, IDictionary<string, List<string>> users);
    IList SearchUsers(string key, Dictionary<string, List<string>> users, string currentUserId);
    string LocateFile(IFormFile file);
    Task<Message> ReadMessage(int id);
    Task<dynamic> GetChat(int id, Dictionary<string, List<string>> users);
    Entities.Chat GetChat(int id);
    List<ChatFile> SaveFiles(int messageId, List<string> files);
    void ClearHistory(int chatId);
    Task RemoveFiles(List<int> fileIds);
    Task<Message> UpdateText(string text, int messageId);
}
