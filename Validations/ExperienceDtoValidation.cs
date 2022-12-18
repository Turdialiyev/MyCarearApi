using FluentValidation;

namespace MyCarearApi.Validations;

public class ExperienceDtoValidation : AbstractValidator<Dtos.FreelancerExperience>
{
    public ExperienceDtoValidation()
    {
        RuleFor(dto => dto.CompanyName).NotEmpty().When(dto => dto.CompanyName != null);
        RuleFor(dto => dto.Descripeion).NotEmpty().When(dto => dto.Descripeion != null);
        RuleFor(dto => dto.Job).NotEmpty().When(dto => dto.Job != null);
    }    
}