using MyCarearApi.Dtos;
using MyCarearApi.Models;

namespace MyCarearApi.Controllers;

public partial class ContractController
{
  
    private Contract ToModelContractDto(CreateContractDto contract, string userId)
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
     DealingDate = DateOnly.FromDateTime(contract.DealingDate!),
     JobId = contract.JobId,
     AppUserId = userId
    };
    
}