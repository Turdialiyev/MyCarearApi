using MyCarearApi.Entities;

namespace MyCarearApi.Hubs
{
    public interface IHubBase
    {
        Task WriteMessage(Message message, List<string> filePaths);
        IAsyncEnumerable<byte> SearchUsers(string key, CancellationToken cancellationToken);
        Task ReadMessage(int id);
        Task DeleteMessage(int messageId);
        Task ClearHistory(int chatId);
        Task RemoveFilesFromMessage(List<int> fileIds, int messageId);
        Task AddFilesToMessage(List<string> paths, int messageId);
        Task UpdateMessageText(string text, int messageId);
    }
}
