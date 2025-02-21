using FluentValidation;
using Project_C_Sharp.Shared.Resources.Validation;
using Project_C_Sharp.Shared.I18n.Validation.Keys;

namespace Project_C_Sharp.Modules.UpdateUser.Dto.Request;

public class UpdateUserRequestDto
{
    public string Name { get; set; } = string.Empty;
}

public class UpdateUserRequestDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Nome"))
            .Length(3, 100).WithMessage(ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Nome").Replace("{1}", "3").Replace("{2}", "100"))
            .Matches("^[a-zA-Z ]*$").WithMessage(ResourceValidation.GetString(ValidationKeys.Nome_OnlyLetters));
    }
}