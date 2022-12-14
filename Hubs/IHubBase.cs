using MyCarearApi.Entities;

namespace MyCarearApi.Hubs
{
    public interface IHubBase
    {
        Task SendMessage(Message message);
        Task SearchUsers(string key);
        Task ReadMessage(int id);
    }
}
