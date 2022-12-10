using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories;
using MyCarearApi.Repositoryies.Interfaces;

namespace MyCarearApi.Repositoryies
{
    public class CurrencyRepository: GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
