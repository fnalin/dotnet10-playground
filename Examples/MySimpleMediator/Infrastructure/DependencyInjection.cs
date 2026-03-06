namespace MySimpleMediator.Infrastructure;

using System.Reflection;
using Mediator;

using Microsoft.Extensions.DependencyInjection;


public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, params Assembly[] assemblies)
    {
        if (assemblies.Length == 0)
        {
            assemblies = [Assembly.GetExecutingAssembly()];
        }

        services.AddScoped<IMediator, SimpleMediator>();

        foreach (var assembly in assemblies)
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

        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));

        return services;
    }
}
