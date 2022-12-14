using MyCarearApi.Entities;
using System.Collections;

namespace MyCarearApi.Hubs
{
    public interface IChatHub
    {
        Task RecieveMessage(Message message);
        Task InitializeChats(IList profils);
        Task IsOnlineChanged(object user);
        Task SearchUser(IList users);
        Task ReadMessage(Message message);

    }
}
