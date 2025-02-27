
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
using Project_C_Sharp.Infra.DataBase.UnitOfWork;


namespace Project_C_Sharp.Modules.Users;

public static class UserModuleExtensions
{
    public static IServiceCollection AddUserModule(
     this IServiceCollection services)
    {
        // Repositories
        services.AddScoped<IUserRepository, UserRepository>();  // AQUI: Mudando de Singleton para Scoped

        // Services
        services.AddScoped<IFindAllUsersService, FindAllUsersService>();
        services.AddScoped<IFindByIdUsersService, FindByIdUsersService>();
        services.AddScoped<ICreateUserService, CreateUsersService>();
        services.AddScoped<IUpdateUsersService, UpdateUsersService>();
        services.AddScoped<IDeleteUsersService, DeleteUsersService>();

        // Validators
        services.AddScoped<IValidator<CreateUserRequestDto>, CreateUserRequestValidator>();
        services.AddScoped<IValidator<UpdateUserRequestDto>, UpdateUserRequestDtoValidator>();

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}