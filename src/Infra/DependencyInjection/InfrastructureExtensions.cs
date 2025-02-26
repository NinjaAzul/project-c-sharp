using Microsoft.EntityFrameworkCore;
using Project_C_Sharp.Infra.DataBaseInMemory;


namespace Project_C_Sharp.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddSingleton<IDataSource, DataSource>();

        return services;
    }
}