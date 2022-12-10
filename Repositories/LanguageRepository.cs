using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories;

public class LanguageRepository:GenericRepository<Language>, ILanguageRepository
{
    public LanguageRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }
}
