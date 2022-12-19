using MyCarearApi.Data;
using MyCarearApi.Entities;
using MyCarearApi.Repositories.Interfaces;

namespace MyCarearApi.Repositories;

public class OfferRepository: GenericRepository<Offer>, IOfferRepository
{
    public OfferRepository(AppDbContext appDbContext): base(appDbContext) { }
}
