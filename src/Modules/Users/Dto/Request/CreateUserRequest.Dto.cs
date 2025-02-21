using FluentValidation;
using Project_C_Sharp.Shared.Resources.Validation;
using Project_C_Sharp.Shared.I18n.Validation.Keys;

namespace Project_C_Sharp.Modules.CreateUser.Dto.Request;

public class CreateUserRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserRequestDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Nome"))
            .Length(3, 100).WithMessage(ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Nome").Replace("{1}", "3").Replace("{2}", "100"))
            .Matches("^[a-zA-Z ]*$").WithMessage(ResourceValidation.GetString(ValidationKeys.Nome_OnlyLetters));

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Email"))
            .EmailAddress().WithMessage(ResourceValidation.GetString(ValidationKeys.Invalid_Email));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Senha"))
            .Length(6, 20).WithMessage(ResourceValidation.GetString(ValidationKeys.Length_Between).Replace("{0}", "Senha").Replace("{1}", "6").Replace("{2}", "20"))
            .Must(password => password.Any(char.IsUpper)).WithMessage(ResourceValidation.GetString(ValidationKeys.Password_RequireUpperCase))
            .Must(password => password.Any(char.IsDigit)).WithMessage(ResourceValidation.GetString(ValidationKeys.Password_RequireNumber));
    }
}