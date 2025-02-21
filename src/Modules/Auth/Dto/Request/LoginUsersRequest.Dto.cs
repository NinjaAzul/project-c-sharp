using FluentValidation;
using Project_C_Sharp.Shared.Resources.Validation;
using Project_C_Sharp.Shared.I18n.Validation.Keys;

namespace Project_C_Sharp.Modules.LoginUsers.Dto.Request;

public class LoginUsersRequestDto
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class LoginUsersRequestDtoValidator : AbstractValidator<LoginUsersRequestDto>
{
    public LoginUsersRequestDtoValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Email"));

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ResourceValidation.GetString(ValidationKeys.Required_Field).Replace("{0}", "Senha"));
    }
}