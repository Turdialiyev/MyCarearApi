using FluentValidation;

namespace MyCarearApi.Validations;

public class PositionDtoValidation : AbstractValidator<Dtos.Position>
{
    public PositionDtoValidation()
    {
        RuleFor(dto => dto.PositionId).NotEmpty().When(dto => dto.PositionId != null).NotEqual(0);
        RuleFor(dto => dto.Description).NotEmpty().When(dto => dto.Description != null);
        RuleForEach(dto => dto.FreelancerSkills).NotEqual(0);
        RuleForEach(dto => dto.FreelancerHobbies).NotEqual(0);
    }
}