using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarrearApi.Data;
using MyCarrearApi.Repositories;

namespace MyCarearApi.Repositoryies
{
    public class AddressRepository: GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
