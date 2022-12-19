
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public partial class ContractService
{
    private MyCareerApi.Entities.Contract ToEntity(Contract contract)
    => new()
    {
        BankINN = contract.BankINN,
        BankName = contract.BankName,
        CardNumber = contract.CardNumber,
        PasportSeriyaNumber = contract.PasportSeriyaNumber,
        MFO = contract.MFO,
        TranzitAccount = contract.TranzitAccount,
        DealingDate = DateOnly.FromDateTime(DateTime.Now),
        INN = contract.INN,
        INPS = contract.INPS,
        JobId = contract.JobId,
        AppUserId = contract.AppUserId
    };
    private Contract ToModel(MyCareerApi.Entities.Contract contract)
    => new()
    {
        Id = contract.Id,
        BankINN = contract.BankINN,
        BankName = contract.BankName,
        CardNumber = contract.CardNumber,
        PasportSeriyaNumber = contract.PasportSeriyaNumber,
        MFO = contract.MFO,
        TranzitAccount = contract.TranzitAccount,
        DealingDate = contract.DealingDate,
        INN = contract.INN,
        INPS = contract.INPS,
        JobId = contract.JobId,
        AppUserId = contract.AppUserId
    };
    private MyCareerApi.Entities.Contract UpdateContract(MyCareerApi.Entities.Contract? existContract, Contract contract)
    {   
        existContract.Id = contract.Id;
        existContract.PasportSeriyaNumber = contract.PasportSeriyaNumber;
        existContract.BankINN = contract.BankINN;
        existContract.BankName = contract.BankName;
        existContract.CardNumber = contract.CardNumber;
        existContract.DealingDate = contract.DealingDate;
        existContract.INN = contract.INN;
        existContract.INPS = contract.INPS;
        existContract.State = contract.State;

        return existContract;
    }
   
}