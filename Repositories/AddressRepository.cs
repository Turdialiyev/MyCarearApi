using MyCareerApi.Entities;
using MyCarearApi.Data;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositories
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
