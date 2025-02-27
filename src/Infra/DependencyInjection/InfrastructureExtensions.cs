using Microsoft.EntityFrameworkCore;
using Project_C_Sharp.Infra.DataBase.EntityFrameworkCore;


namespace Project_C_Sharp.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
    {
        // Adicione logging para debug
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"))
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors());

        return services;
    }
}