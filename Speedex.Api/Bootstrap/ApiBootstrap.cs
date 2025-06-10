using Speedex.Data;
using Speedex.Data.Generators;
using Speedex.Domain.Returns;

namespace Speedex.Api.Bootstrap;

public static class ApiBootstrap
{
    public static IServiceCollection RegisterApiServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .RegisterDataGenerators(configuration);

        return services;
    }

    private static IServiceCollection RegisterDataGenerators(this IServiceCollection services, ConfigurationManager configuration)
    {
        services
            .AddSingleton<IDataGenerator, DataGenerator>()
            .AddSingleton<IDataGenerator<ReturnId, Return>, ReturnsGenerator>();

        services
            .Configure<GenerateOptions>(configuration.GetSection(GenerateOptions.SectionName));

        return services;
    }
}