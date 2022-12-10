using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class CurrencyRepository: GenericRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
