using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Services.Chat;

public interface IMessageService
{
    Message AddMessage(Message message);
    Task<Message> UpdateMessage(Message message);
    void RemoveMessage(int Id);
    IList GetChats(string userId);
}
