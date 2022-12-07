using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class ResumeRepository : GenericRepository<Resume>, IResumeRepository
{
    public ResumeRepository(AppDbContext context) : base(context)
    {
    }
}