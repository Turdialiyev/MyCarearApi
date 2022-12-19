using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Hubs
{
    public interface IChatHub
    {
        Task RecieveMessage<T>(T message);
        Task IsOnlineChanged(object user);
        Task ReadMessage<T>(T message);
        Task InitializeChats();
        Task MessageRemoved<T>(T removedMessageInfo);
        Task HistoryCleared<T>(T chatInfo);
        Task MessageEdited<T>(T message);
    }
}
