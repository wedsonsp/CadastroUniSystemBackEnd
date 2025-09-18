using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Sistemaws.Application.Commands;
using Sistemaws.Application.Handlers;
using Sistemaws.Application.Queries;
using Sistemaws.Application.Validators;

namespace Sistemaws.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // MediatR
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
        });

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(ServiceCollectionExtensions).Assembly);

        return services;
    }
}
