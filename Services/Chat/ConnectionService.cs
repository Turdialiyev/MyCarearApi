using MyCarearApi.Data;
using MyCarearApi.Services.Chat.Interfaces;
using MyCarearApi.Entities.ChatEntities;

namespace MyCarearApi.Services.Chat
{
    public class ConnectionService: IConnectionService
    {
        private ChatDbContext _context;

        public ConnectionService(ChatDbContext context)
        {
            _context = context;
        }

        public void AddConnection(string connectionId, string userId)
        {
            _context.Connections.Add(new Connection { ConnectionId= connectionId, UserId = userId });
            _context.SaveChanges();
        }

        public List<string> GetUserIds() => _context.Connections.Select(x => x.UserId).Distinct().ToList();

        public List<string> GetConnections(string userId)
        {
            return _context.Connections.Where(x => x.UserId== userId).Select(x => x.ConnectionId).ToList();
        }

        public void RemoveConnection(string connectionId)
        {
            var connection = _context.Connections.Where(x => x.ConnectionId == connectionId).FirstOrDefault();
            if (connection is null) return;
            _context.Connections.Remove(connection);
        }
    }
}
