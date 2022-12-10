using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;

namespace MyCarearApi.Repositoryies;

public class LanguageRepository:GenericRepository<Language>, ILanguageRepository
{
    public LanguageRepository(AppDbContext appDbContext) : base(appDbContext)
    {

    }
}
