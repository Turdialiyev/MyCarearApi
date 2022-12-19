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
          
          var position = _unitOfWork.Positions.GetById(job.PositionsId!.Value);
          var currency = _unitOfWork.Currencies.GetById(2);

          var dagavor = new Dogovor();
          dagavor.ContractDate = contract.DealingDate;
          dagavor.FreelancerName = freelancer.FirstName + " " + freelancer.LastName;
          dagavor.PassportSeria = contract.PasportSeriyaNumber;
          dagavor.JobTitle = job.Name;
          dagavor.Position = position!.Name;
          dagavor.JobDescription = job.Description;
          dagavor.FreelancerName = freelancer.LastName!.ToString();
          dagavor.Summa =  "443354565673"+" ( "+ CalculateSumma.Calculate(443354565673)+ ")"+$"  {currency.Name}  {currency.Code}";
          dagavor.Diedline = DateOnly.FromDateTime(DateTime.Now.AddDays(job.DeadLine!.Value));

        // dagavor.AdvancePayment = 
        // dagavor.LastPayment = 
         
         return new(true) { Data = dagavor};
       }
       catch (System.Exception)
       {
        
        throw;
       }
    }

    // https://www.signwell.com/online-signature/ -- bu elektron imzo uchin tekin sayt
    // https://smallpdf.com/sign-pdf -- buham
    // https://www.digisigner.com/free-electronic-signature/sign-document-online --bu eng zori

    public async  ValueTask<Result<string>> SaveContract(string fileUrl)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot",Guid.NewGuid().ToString().Substring(27)+".jpg");
        var converter = new HtmlConverter();

        var bytes = converter.FromUrl(fileUrl);
        File.WriteAllBytes(path, bytes);
        // HtmlToPdf();
        
        return new(true) {Data = path};
    }

    // private void HtmlToPdf()
    // {
    //      SautinSoft.PdfMetamorphosis p = new SautinSoft.PdfMetamorphosis();

    //         p.PageSettings.Size.A4();
    //         p.PageSettings.Orientation = SautinSoft.PdfMetamorphosis.PageSetting.Orientations.Landscape;
    //         p.PageSettings.Numbering.Text = "Page {page} of {numpages}";

    //         if (p != null)
    //         {
    //             string inputFile = @"sample2.html";
    //             string outputFile = @"test.pdf";

    //             int result = p.HtmlToPdfConvertFile(inputFile, outputFile);


    //             if (result == 0)
    //             {
    //                 System.Console.WriteLine("Converted successfully!");
    //                 System.Diagnostics.Process.Start(outputFile);
    //             }
    //             else
    //             {
    //                 System.Console.WriteLine("Converting Error!");
    //             }
    // }}
    
    }