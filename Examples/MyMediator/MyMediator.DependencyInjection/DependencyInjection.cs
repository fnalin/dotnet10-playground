using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyMediator.Core;

namespace MyMediator.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddMySimpleMediator(
        this IServiceCollection services,
        Action<MediatorOptions> configure)
    {
        var options = new MediatorOptions();
        configure(options);

        services.AddScoped<IMediator, SimpleMediator>();

        foreach (var assembly in options.Assemblies.Distinct())
        {
            RegisterAssemblyServices(services, assembly);
        }

        foreach (var behavior in options.OpenBehaviors)
        {
            services.AddScoped(typeof(IPipelineBehavior<,>), behavior);
        }

        return services;
    }

    private static void RegisterAssemblyServices(IServiceCollection services, Assembly assembly)
    {
        foreach (var type in assembly.GetTypes())
        {
            if (type.IsAbstract || type.IsInterface)
                continue;

            foreach (var contract in type.GetInterfaces())
            {
                if (!contract.IsGenericType)
                    continue;

                var genericType = contract.GetGenericTypeDefinition();

                if (genericType == typeof(IRequestHandler<,>) ||
                    genericType == typeof(INotificationHandler<>) ||
                    genericType == typeof(IValidator<>))
                {
                    services.AddScoped(contract, type);
                }
            }
        }
    }
}