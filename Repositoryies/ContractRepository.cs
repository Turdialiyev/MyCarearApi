using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarrearApi.Data;
using MyCarrearApi.Repositories;

namespace MyCarearApi.Repositoryies
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
