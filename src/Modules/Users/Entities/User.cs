using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Shared.AuditableEntity.Entities;
using Project_C_Sharp.Shared.Exceptions;
using Project_C_Sharp.Shared.Validation;

namespace Project_C_Sharp.Modules.Users.Entities;


[Table("Users")]
public class User : AuditableEntity
{
    [Key]
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(100)]
    [MinLength(3)]
    public string Name { get; private set; }

    [Required]
    [EmailAddress]
    public string Email { get; private set; }

    [Required]
    [StringLength(100)]
    public string Password { get; private set; }

    private User(string name, string email, string password)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        Password = password;
    }

    public static User Create(CreateUserRequestDto request, IValidator<CreateUserRequestDto> validator)
    {
        var validationResult = validator.Validate(request);
        if (!validationResult.IsValid)
        {
            throw new BadRequestException(validationResult.Errors);
        }

        var user = new User(request.Name, request.Email, request.Password);
        user.GeneratePasswordHash(request.Password);

        return user;
    }

    private void GeneratePasswordHash(string password)
    {
        Password = BCrypt.Net.BCrypt.HashPassword(password);
    }

    public void UpdateName(string name)
    {
        Name = name;
    }
}

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).MustBeValidName();
        RuleFor(x => x.Email).MustBeValidEmail();
        RuleFor(x => x.Password).MustBeValidPassword();
    }
}
