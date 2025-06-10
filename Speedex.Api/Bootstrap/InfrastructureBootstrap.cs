using Speedex.Domain.Returns.Repositories;
using Speedex.Infrastructure;

namespace Speedex.Api.Bootstrap;

public static class InfrastructureBootstrap
{
    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services)
    {
        services
            .RegisterRepositories();

        return services;
    }

    private static IServiceCollection RegisterRepositories(this IServiceCollection services)
    {
        services
            .AddSingleton<IReturnRepository, InMemoryReturnRepository>();

        return services;
    }
}