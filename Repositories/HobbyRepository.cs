using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class HobbyRepository : GenericRepository<Hobby>, IHobbyRepository
{
    public HobbyRepository(AppDbContext context) : base(context) { }
}