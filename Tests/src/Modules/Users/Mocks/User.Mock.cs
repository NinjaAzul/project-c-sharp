using Bogus;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.Users.Entities;
using Project_C_Sharp.Modules.UsersBasicInfo.DTOs.Response;
using Project_C_Sharp.Shared.Exceptions;

namespace Project_C_Sharp.Tests.Modules.Users.Mocks;

public static class UserMocks
{


    private static readonly Faker<User> UserFaker = new Faker<User>("pt_BR")
       .CustomInstantiator(f => User.Create(
           new CreateUserRequestDto
           {
               Name = RemoveAccents(f.Name.FirstName() + " " + f.Name.LastName()),
               Email = f.Internet.Email(),
               Password = $"Password{f.Random.Number(100, 999)}"
           },
           new CreateUserRequestValidator()
       ));

    private static readonly string[] ValidNames =
    [
        "Isabel Silva",
        "Maria Santos",
        "Jose Carlos",
        "Ana Paula"
    ];

    private static readonly Faker<CreateUserRequestDto> CreateUserDtoFaker = new Faker<CreateUserRequestDto>("pt_BR")
        .RuleFor(u => u.Name, f => f.PickRandom(ValidNames))
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => $"Password{f.Random.Number(100, 999)}");

    public static class Valid
    {
        public static User GenerateUser()
        {
            try
            {
                var user = UserFaker.Generate();
                return user;
            }
            catch (BadRequestException ex)
            {
                Console.WriteLine($"Validation failed for: {ex.Message}");
                if (ex.Errors != null)
                {
                    foreach (var error in ex.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                throw;
            }
        }

        public static List<UserBasicInfoResponseDto> GenerateBasicResponseCrudDto(int count = 1) => UserFaker.Generate(count).Select(user => new UserBasicInfoResponseDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email
        }).ToList();
        public static List<User> GenerateUsers(int count = 3) => UserFaker.Generate(count);
        public static CreateUserRequestDto GenerateCreateDto()
        {
            try
            {
                var dto = CreateUserDtoFaker.Generate();
                // Console.WriteLine("DTO gerado com sucesso:");
                // Console.WriteLine($"Name: {dto.Name}");
                // Console.WriteLine($"Email: {dto.Email}");
                // Console.WriteLine($"Password: {dto.Password}");
                return dto;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar DTO: {ex.Message}");
                if (ex is BadRequestException badRequest && badRequest.Errors != null)
                {
                    foreach (var error in badRequest.Errors)
                    {
                        Console.WriteLine($"- {error.ErrorMessage}");
                    }
                }
                throw;
            }
        }

        public static string Name => "John Silva";
        public static string Email => "john.doe@example.com";
        public static string Password => "Password123";
    }

    public static class Invalid
    {
        public static class Name
        {
            public static readonly string Empty = string.Empty;
            public static readonly string TooShort = "Jo";
            public static readonly string WithSpecialCharacters = "John@ Doe!";
        }

        public static class Email
        {
            public static readonly string Empty = "";
            public static readonly string Invalid = "invalid.email";
            public static readonly string WithoutDomain = "email@";
        }

        public static class Password
        {
            public static readonly string Empty = "";
            public static readonly string TooShort = "Pass1";
            public static readonly string WithoutUpperCase = "password123";
            public static readonly string WithoutNumber = "Password";
        }


    }

    private static string RemoveAccents(string text)
    {
        var normalizedString = text.Normalize(System.Text.NormalizationForm.FormD);
        var stringBuilder = new System.Text.StringBuilder();

        foreach (var c in normalizedString)
        {
            if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(System.Text.NormalizationForm.FormC);
    }
}