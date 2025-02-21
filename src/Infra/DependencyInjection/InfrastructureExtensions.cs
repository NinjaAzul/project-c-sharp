using Microsoft.EntityFrameworkCore;


namespace Project_C_Sharp.Infrastructure.DependencyInjection;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {


        return services;
    }
}