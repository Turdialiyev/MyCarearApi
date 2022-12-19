using MyCarearApi.Dtos;
using MyCarearApi.Models;

namespace MyCarearApi.Controllers;

public partial class ContractController
{
    private ReturnCreatedContract ContractModelToDto(Contract contract)
    => new()
    {
     Id = contract.Id,
     PasportSeriyaNumber = contract.PasportSeriyaNumber,
     INN = contract.INN,
     BankName = contract.BankName,
     BankINN = contract.BankINN,
     TranzitAccount = contract.TranzitAccount,
     CardNumber = contract.CardNumber,
     INPS = contract.INPS,
     MFO = contract.MFO,
     State = contract.State,
     DealingDate = contract.DealingDate,
     JobId = contract.JobId,
     AppUserId = contract.AppUserId
    };

    private Contract ToModelContractDto(CreateContractDto contract)
    => new()
    {
     Id = contract.Id,
     PasportSeriyaNumber = contract.PasportSeriyaNumber,
     INN = contract.INN,
     BankName = contract.BankName,
     BankINN = contract.BankINN,
     TranzitAccount = contract.TranzitAccount,
     CardNumber = contract.CardNumber,
     INPS = contract.INPS,
     MFO = contract.MFO,
     State = contract.State,
     DealingDate = contract.DealingDate,
     JobId = contract.JobId,
     AppUserId = contract.AppUserId
    };
    
}