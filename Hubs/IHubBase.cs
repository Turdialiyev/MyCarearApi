using MyCarearApi.Entities;

namespace MyCarearApi.Hubs
{
    public interface IHubBase
    {
        Task WriteMessage(Message message);
        IAsyncEnumerable<byte> SearchUsers(string key, CancellationToken cancellationToken);
        Task ReadMessage(int id);
    }
}
