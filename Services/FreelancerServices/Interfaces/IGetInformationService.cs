
using MyCarearApi.Models;

namespace MyCarearApi.Services;

public interface IGetInformationService
{
    ValueTask<Result<List<Entities.Position>>> GetPositions();
    ValueTask<Result<List<Entities.Country>>> GetCountries();
    ValueTask<Result<List<Entities.Language>>> GetLanguages();
    ValueTask<Result<List<Entities.Hobby>>> GetHobbies();

}