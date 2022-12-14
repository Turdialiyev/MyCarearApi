using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories.Interfaces;

namespace MyCarearApi.Repositories
{
    public class ChatRepository: GenericRepository<Chat>, IChatRepository
    {
        public ChatRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
