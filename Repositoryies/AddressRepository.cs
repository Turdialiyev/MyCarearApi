using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarearApi.Data;
using MyCarearApi.Repositories;
using MyCarearApi.Entities;

namespace MyCarearApi.Repositoryies
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
