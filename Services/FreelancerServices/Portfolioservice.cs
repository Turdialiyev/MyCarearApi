using Microsoft.AspNetCore.Identity;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class PortfolioService : IPortfolioService
{
    private readonly ILogger<PortfolioService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<Entities.AppUser> _userManager;
    private readonly IFileHelper _fileHelper;

    public PortfolioService(
        ILogger<PortfolioService> logger,
        IUnitOfWork unitOfWork,
        UserManager<Entities.AppUser> userManager,
        IFileHelper fileHelper
        )
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
        _userManager = userManager;
        _fileHelper = fileHelper;
    }
    public async ValueTask<Result<FreelancerPortfolio>> SaveAsync(string userId, IFormFile image, FreelancerPortfolio portfolio)
    {
        string? imageName = null;

        try
        {
            if (portfolio is null)
                return new(false) { ErrorMessage = "Portfolio Null exception" };

            if (string.IsNullOrWhiteSpace(userId))
                return new(false) { ErrorMessage = "User Id invalid" };

            var existUser = await _userManager.FindByIdAsync(userId);

            if (existUser is null)
                return new(false) { ErrorMessage = "User Id not found" };

            if (image is not null)
            {
                try
                {
                    if (!_fileHelper.FileValidateImage(image))
                        return new("File invalid recive only picture");

                    imageName = await _fileHelper.WriteFileAsync(image, FileFolders.PortfolioImage);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            var result = await _unitOfWork.FreelancerPortfolios.AddAsync(ToEntity(userId, imageName, portfolio));


            return new(true) { Data = ToModel(result) };

        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(PortfolioService)}", e);

            throw new("Couldn't create Portfolio. Plaese contact support", e);
        }
    }

    private FreelancerPortfolio ToModel(Entities.FreelancerPortfolio result) => new()
    {
        Id = result.Id,
        FirstName = result.FirstName,
        LastName = result.LastName,
        Price = result.Price,
        Available = result.Available,
        ImageName = result.ImageName,
        PositionId = result.PositionId,
        PositionName = result.Position == null ? null : result.Position.Name,
    };

    private Entities.FreelancerPortfolio ToEntity(string userId, string? imageName, FreelancerPortfolio portfolio) => new()
    {
        FirstName = portfolio.FirstName,
        LastName = portfolio.LastName,
        PositionId = portfolio.PositionId,
        Price = portfolio.Price,
        ImageName = imageName,
        AppUserId = userId

    };

    public async ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, IFormFile image, FreelancerPortfolio portfolio)
    {

        string? filePath = null;
        try
        {
            if (Id <= 0)
                return new(false) { ErrorMessage = "Id is invalid" };

            var existPortfolio = _unitOfWork.FreelancerPortfolios.GetById(Id);

            if (existPortfolio is not null)
                return new(false) { ErrorMessage = "Portfolio is not found" };

            if (portfolio is not null)
                return new(false) { ErrorMessage = "portfolio Null exseption" };
            try
            {
                if (File.Exists(existPortfolio!.ImageName))
                    _fileHelper.DeleteFileByName(existPortfolio.ImageName!);

                if (!_fileHelper.FileValidateImage(image))
                    return new("File is invalid recive only picture");

                if (image is not null)
                    filePath = await _fileHelper.WriteFileAsync(image, FileFolders.PortfolioImage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            existPortfolio!.FirstName = portfolio!.FirstName;
            existPortfolio.LastName = portfolio.LastName;
            existPortfolio.PositionId = portfolio.PositionId;
            existPortfolio.Price = portfolio.Price;
            existPortfolio.ImageName = filePath;

            var result = await _unitOfWork.FreelancerPortfolios.Update(existPortfolio);

            return new(true) { Data = ToModel(result) };
        }
        catch (Exception e)
        {

            _logger.LogError($"Error occured at {nameof(PortfolioService)}", e);

            throw new("Couldn't Portfolio update. Plaese contact support", e);
        }
    }

    public async ValueTask<Result<FreelancerPortfolio>> UpdateAsync(int Id, string available)
    {
        try
        {
            if (Id <= 0)
                return new(false) { ErrorMessage = "Id is invalid" };

            if (!string.IsNullOrWhiteSpace(available))
                return new(false) { ErrorMessage = "Available is invalid" };

            var existPortfolio = _unitOfWork.FreelancerPortfolios.GetById(Id);

            if (existPortfolio is not null)
                return new(false) { ErrorMessage = "Portfolio is not found" };

            existPortfolio!.Available = available;

            var result = await _unitOfWork.FreelancerPortfolios.Update(existPortfolio);

            return new(true) { Data = ToModel(result) };
        }
        catch (Exception e)
        {
            _logger.LogError($"Error occured at {nameof(PortfolioService)}", e);

            throw new("Couldn't update Portfolio Available. Plaese contact support", e);
        }
    }
}