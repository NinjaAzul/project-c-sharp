using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Project_C_Sharp.Shared.Configs.Validations.AssemblyMaker;

namespace Project_C_Sharp.Shared.Configuration.Validations;

public static class ValidationConfig
{
    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        // Configura o modo cascade global
        ValidatorOptions.Global.DefaultRuleLevelCascadeMode = CascadeMode.Continue;

        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblyContaining<AssemblyMarker>();

        return services;
    }
}