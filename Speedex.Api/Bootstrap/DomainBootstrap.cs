using FluentValidation;
using Speedex.Domain.Commons;
using Speedex.Domain.Returns.UseCases.CreateReturn;
using Speedex.Domain.Returns.UseCases.GetReturns;

namespace Speedex.Api.Bootstrap;

public static class DomainBootstrap
{
    public static IServiceCollection RegisterDomainServices(this IServiceCollection services)
    {
        services
            .RegisterCommandHandlers()
            .RegisterQueryHandlers()
            .RegisterValidators();

        return services;
    }

    private static IServiceCollection RegisterCommandHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<ICommandHandler<CreateReturnCommand, CreateReturnResult>, CreateReturnCommandHandler>();

        return services;
    }

    private static IServiceCollection RegisterValidators(this IServiceCollection services)
    {
        services
            .AddScoped<Domain.Commons.IValidator<CreateReturnCommand>, CreateReturnCommandValidator>();
        return services;
    }

    private static IServiceCollection RegisterQueryHandlers(this IServiceCollection services)
    {
        services
            .AddScoped<IQueryHandler<GetReturnsQuery, GetReturnsQueryResult>, GetReturnsQueryHandler>();
        return services;
    }
}