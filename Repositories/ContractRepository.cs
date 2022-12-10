using MyCarearApi.Repositoryies.Interfaces;
using MyCareerApi.Entities;
using MyCarearApi.Data;
using MyCarearApi.Repositories;
using MyCarearApi.Repositories.Interfaces;

namespace MyCarearApi.Repositoryies
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
