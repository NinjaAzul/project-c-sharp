using FluentValidation;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.I18n.Validation.Keys;
using Project_C_Sharp.Shared.Resources.Validation;

namespace Project_C_Sharp.Modules.Users.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;
    public string Password { get; private set; } = null!;

    public User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;

        Validate();
    }

    public void GeneratePasswordHash()
    {
        Password = BCrypt.Net.BCrypt.HashPassword(Password);
    }

    public void UpdateName(string name)
    {
        Name = name;
        Validate();
    }

    private void Validate()
    {
        var validator = new UserValidator();
        var validationResult = validator.Validate(this);

        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors);
        }
    }


}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
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
