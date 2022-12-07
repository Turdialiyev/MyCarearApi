using MyCarearApi.Data;
using MyCareerApi.Entities;

namespace MyCarearApi.Repositories;

public class ResumeRepository : GenericRepository<Resume>, IResumeRepository
{
    public ResumeRepository(AppDbContext context) : base(context)
    {
    }
}