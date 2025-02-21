
using FluentValidation;
using Project_C_Sharp.Modules.Users.Repositories;
using Project_C_Sharp.Modules.Users.Repositories.Interfaces;
using Project_C_Sharp.Modules.Users.Services.CreateUsers;
using Project_C_Sharp.Modules.Users.Services.CreateUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers;
using Project_C_Sharp.Modules.Users.Services.DeleteUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers;
using Project_C_Sharp.Modules.Users.Services.FindAllUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers;
using Project_C_Sharp.Modules.Users.Services.FindByIdUsers.Interfaces;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers;
using Project_C_Sharp.Modules.Users.Services.UpdateUsers.Interfaces;
using Project_C_Sharp.Modules.CreateUser.Dto.Request;
using Project_C_Sharp.Modules.UpdateUser.Dto.Request;


namespace Project_C_Sharp.Modules.Users;

public static class UserModuleExtensions
{
    public static IServiceCollection AddUserModule(
     this IServiceCollection services)
    {
        // Repositories
        services.AddSingleton<IUserRepository, UserRepository>();
        // Services
        services.AddScoped<IFindAllUsersService, FindAllUsersService>();
        services.AddScoped<IFindByIdUsersService, FindByIdUsersService>();
        services.AddScoped<ICreateUserService, CreateUsersService>();
        services.AddScoped<IUpdateUsersService, UpdateUsersService>();
        services.AddScoped<IDeleteUsersService, DeleteUsersService>();

        // Validators
        services.AddScoped<IValidator<CreateUserRequestDto>, CreateUserRequestDtoValidator>();
        services.AddScoped<IValidator<UpdateUserRequestDto>, UpdateUserRequestDtoValidator>();


        return services;
    }
}