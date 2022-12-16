using FluentValidation;

namespace MyCarearApi.Validations;

public class AdressDtoValidation : AbstractValidator<Dtos.Adress>
{
    public AdressDtoValidation()
    {
        RuleFor(dto => dto.CountryId).NotNull().NotEqual(0);
        RuleFor(dto => dto.RegionId).NotNull().NotEqual(0);
        RuleFor(dto => dto.Home).NotEmpty();
    }
}