using MyCareerApi.Entities;
using MyCarearApi.Data;

namespace MyCarearApi.Repositories
{
    public class ContractRepository: GenericRepository<Contract>, IContractRepository
    {
        public ContractRepository(AppDbContext appDbContext) : base(appDbContext) { }
    }
}
