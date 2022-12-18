using FluentValidation;

namespace MyCarearApi.Validations;

public class EducationDtoValidation : AbstractValidator<Dtos.FreelancerEducation>
{
    public EducationDtoValidation()
    {
        RuleFor(dto => dto.Location).NotEmpty().When(dto => dto.Location != null);
        RuleFor(dto => dto.SchoolName).NotEmpty().When(dto => dto.SchoolName != null);
        RuleFor(dto => dto.TypeStudy).NotEmpty().When(dto => dto.TypeStudy != null);
        RuleFor(dto => dto.EducationDegree).NotEmpty().When(dto => dto.EducationDegree != null);
    }    
}