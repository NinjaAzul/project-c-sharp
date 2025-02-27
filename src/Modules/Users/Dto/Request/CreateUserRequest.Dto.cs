using FluentValidation;
using Project_C_Sharp.Shared.Validation;

namespace Project_C_Sharp.Modules.CreateUser.Dto.Request;

public class CreateUserRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class CreateUserRequestValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserRequestValidator()
    {
        RuleFor(x => x.Name).MustBeValidName();
        RuleFor(x => x.Email).MustBeValidEmail();
        RuleFor(x => x.Password).MustBeValidPassword();
    }
}