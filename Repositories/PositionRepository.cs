using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class PositionRepository : GenericRepository<Position>, IPositionRepository
{
    public PositionRepository(AppDbContext context) : base(context) { }
}