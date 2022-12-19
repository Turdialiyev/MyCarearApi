using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IContractService
{
    ValueTask<Result<Contract>> CreateContract(Contract contract);
    ValueTask<Result<string>> SaveContract(string fileUrl);
    ValueTask<Result<Dogovor>> GetDagovorItems(int contractId);
    
}