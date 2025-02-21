using Project_C_Sharp.Modules.Auth;
using Project_C_Sharp.Modules.Users;

namespace Project_C_Sharp.Modules.DependencyInjection;

public static class ModulesExtensions
{
    public static IServiceCollection AddModules(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddUserModule();
        services.AddAuthModule(configuration);
        return services;
    }
}