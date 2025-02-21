
using FluentValidation;
using Project_C_Sharp.Modules.Auth.Services.LoginUsers;
using Project_C_Sharp.Modules.Auth.Services.LoginUsers.Interfaces;
using Project_C_Sharp.Modules.Auth.Services.Me.Interfaces;
using Project_C_Sharp.Modules.Auth.Services.Token;
using Project_C_Sharp.Modules.Auth.Services.UserData;
using Project_C_Sharp.Modules.LoginUsers.Dto.Request;
using Project_C_Sharp.Modules.Users.Repositories;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Shared.Configs.Jwt;



namespace Project_C_Sharp.Modules.Auth;

public static class AuthModuleExtensions
{
    public static IServiceCollection AddAuthModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {

        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        // Repositories
        services.AddSingleton<IUserRepository, UserRepository>();
        // Services
        services.AddScoped<ILoginUsersService, LoginUsersService>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserDataService, UserDataService>();
        // Validators
        services.AddScoped<IValidator<LoginUsersRequestDto>, LoginUsersRequestDtoValidator>();


        return services;
    }
}