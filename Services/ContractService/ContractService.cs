using CoreHtmlToImage;
using Microsoft.AspNetCore.Identity;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public partial class ContractService : IContractService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ContractService> _logger;
    private readonly UserManager<Entities.AppUser> _appUser;

    public ContractService(IUnitOfWork unitOfWork,
     ILogger<ContractService> logger,
     UserManager<Entities.AppUser> appUser)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _appUser = appUser;
    }
    public async ValueTask<Result<Contract>> CreateContract(Contract contract)
    {
        try
        {
            if(contract is null)
            return new("contact  can't be null ");

            if(contract.Id != 0)
            {
               var existContract = _unitOfWork.Contracts.GetById(contract.Id);

               if( existContract is not null)
               {
                var updatedContract = await _unitOfWork.Contracts.Update(UpdateContract(existContract, contract));
                
                if(updatedContract is null)
                return new("Contract is not updated");


                return new(true) { Data = ToModel(updatedContract)};
               }

            }

            var createdConract = await _unitOfWork.Contracts.AddAsync(ToEntity(contract));

            if(createdConract is null)
            return new("Contract did not created");

            return new(true) { Data = ToModel(createdConract)};
        }
        catch (System.Exception e)
        {
            _logger.LogInformation($"Contract didn't created {contract}");
            throw new Exception(e.Message);
        }
    }

    public async ValueTask<Result<Dogovor>> GetDagovorItems(int contractId)
    {
       try
       {
          var contract = _unitOfWork.Contracts.GetById(contractId);

          if(contract is null)
          return new("This contract is not found");

          var freelancer = _appUser.Users.FirstOrDefault(x => x.Id == contract.AppUserId);
          if(freelancer is null)
          return new("Freelancer is not found");
          
          var job = _unitOfWork.Jobs.GetById(contract.JobId);
          
          var position = _unitOfWork.Jobs.GetById(job.PositionsId);

          var dagavor = new Dogovor();
          dagavor.ContractDate = DateOnly.MaxValue;
          dagavor.FreelancerName = freelancer.FirstName + " " + freelancer.LastName;
          dagavor.PassportSeria = contract.PasportSeriyaNumber;
          dagavor.JobTitle = job.Name;
          dagavor.Position = position.Name;
          dagavor.JobDescription = job.Description;
          dagavor.Summa =  job.Price.ToString()+" ( "+ CalculateSumma.Calculate(job.Price)+" )";
          dagavor.Diedline = DateOnly.FromDateTime(DateTime.Now);
        // dagavor.AdvancePayment = 
        // dagavor.LastPayment = 
         
         return new(true) { Data = dagavor};
       }
       catch (System.Exception)
       {
        
        throw;
       }
    }

    

    // public DateOnly ContractDate {get; set;}
    // public string? FreelancerName { get; set; }
    // public string? PassportSeria { get; set; }
    // public string? JobTitle { get; set; }
    // public string? Position { get; set; }
    // public string? JobDescription { get; set; }
    // public string? Summa { get; set; }
    // public DateOnly Diedline { get; set; }
    // public Decimal AdvancePayment { get; set; }
    // public Decimal LastPayment {get; set;}
    public async  ValueTask<Result<string>> SaveContract(string fileUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",Guid.NewGuid().ToString().Substring(27)+"jpg");
        var converter = new HtmlConverter();

        var bytes = converter.FromUrl(fileUrl);
        File.WriteAllBytes(path, bytes);
        
        return new(true) {Data = path};
    }
}