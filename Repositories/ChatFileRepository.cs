using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories.Interfaces;

namespace MyCarearApi.Repositories;

public class ChatFileRepository : GenericRepository<ChatFile>, IChatFileRepository
{
    public ChatFileRepository(AppDbContext appDbContext) : base(appDbContext) { }
}
