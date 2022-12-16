using FluentValidation;

namespace MyCarearApi.Validations;

public class ContactDtoValidation : AbstractValidator<Dtos.FreelancerContact>
{
    public ContactDtoValidation()
    {
        RuleFor(dto => dto.WebSite).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.WatsApp).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.Facebook).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.GitHub).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.Instagram).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.Twitter).NotEmpty().When(dto => dto != null);
        RuleFor(dto => dto.Telegram).NotEmpty().When(dto => dto != null);
    }    
}