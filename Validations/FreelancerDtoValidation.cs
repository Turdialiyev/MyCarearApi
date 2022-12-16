using FluentValidation;

namespace MyCarearApi.Validations;

public class FreelancerDtoValidation : AbstractValidator<Dtos.Freelancer>
{
    public FreelancerDtoValidation()
    {
        RuleFor(dto => dto.FirstName).NotEmpty().NotNull();
        RuleFor(dto => dto.LastName).NotEmpty().NotNull();
        RuleFor(dto => dto.Email).NotEmpty().NotNull().EmailAddress();
        RuleFor(dto => dto.Phone).NotEmpty().NotNull();
    }
}