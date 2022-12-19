using FluentValidation;

namespace MyCarearApi.Validations;

public class LanguageDtoValidation : AbstractValidator<Dtos.FreelancerLanguage>
{
    public LanguageDtoValidation()
    {
        RuleFor(dto => dto.LanguageId).NotEqual(0).NotEmpty().When(dto => dto.LanguageId != null);
        RuleFor(dto => dto.Level).NotEmpty().When(dto => dto.Level != null);
    }
}