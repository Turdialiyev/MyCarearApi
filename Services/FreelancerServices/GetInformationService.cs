# pragma warning disable
using Microsoft.EntityFrameworkCore;
using MyCarearApi.Entities;
using MyCarearApi.Models;
using MyCarearApi.Repositories;

namespace MyCarearApi.Services;

public class GetInformationService : IGetInformationService
{
    private readonly IUnitOfWork _unitOfWork;

    public GetInformationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async ValueTask<Result<List<Country>>> GetCountries()
    {
        try
        {
            var result = await _unitOfWork.Countries.GetAll().Include(x => x.Regions).ToListAsync();

            if (!result.Any())
                return new(false) { ErrorMessage = "Any Hobby not found" };

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new($"Couldn't get Hobbiess Please contact support");
        }
    }

    public async ValueTask<Result<List<Entities.Hobby>>> GetHobbies()
    {
        try
        {
            var result = await _unitOfWork.Hobbies.GetAll().ToListAsync();

            if (!result.Any())
                return new(false) { ErrorMessage = "Any Hobby not found" };

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new($"Couldn't get Hobbiess Please contact support");
        }
    }

    public async ValueTask<Result<List<Entities.Language>>> GetLanguages()
    {
        try
        {
            var result = await _unitOfWork.Languages.GetAll().ToListAsync();

            if (!result.Any())
                return new(false) { ErrorMessage = "Any Languages not found" };

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new($"Couldn't get Languages Please contact support");
        }
    }

    public async ValueTask<Result<List<Entities.Position>>> GetPositions()
    {
        try
        {
            var result = await _unitOfWork.Positions.GetAll().Include(x => x.Skills).ToListAsync();

            if (!result.Any())
                return new(false) { ErrorMessage = "Any Languages not found" };

            return new(true) { Data = result };
        }
        catch (Exception e)
        {
            throw new($"Couldn't get Languages Please contact support");
        }
    }

    public async Task<Result<List<Entities.Skill>>> GetSkills()
        => new Result<List<Entities.Skill>>(true) { Data = await _unitOfWork.Skills.GetAll().ToListAsync() };

    public async Task<Result<List<Entities.Currency>>> GetCurrencies() 
        => new(true) { Data = await _unitOfWork.Currencies.GetAll().ToListAsync() };
}