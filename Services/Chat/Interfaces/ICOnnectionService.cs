namespace MyCarearApi.Services.Chat.Interfaces
{
    public interface IConnectionService
    {
        List<string> GetConnections(string userId);
        List<string> GetUserIds();

        void AddConnection(string connectionId, string userId);
        void RemoveConnection(string connectionId);
    }
}
