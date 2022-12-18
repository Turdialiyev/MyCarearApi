using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities.ChatEntities;

namespace MyCarearApi.Data;

public class ChatDbContext: DbContext
{
    public ChatDbContext(DbContextOptions<ChatDbContext> options) : base(options) { }

    public DbSet<Connection> Connections { get; set; }
}
