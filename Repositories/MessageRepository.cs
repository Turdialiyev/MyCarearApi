using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class MessageRepository: GenericRepository<Message>, IMessageRepository
    {
        public MessageRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
