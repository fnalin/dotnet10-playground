using Microsoft.Extensions.DependencyInjection;

namespace MyMediator.Core;

public sealed class SimpleMediator(IServiceProvider serviceProvider) : IMediator
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    public async Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        var requestType = request.GetType();
        var responseType = typeof(TResponse);

        var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
        var handler = _serviceProvider.GetRequiredService(handlerType);

        var behaviorType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
        var behaviors = _serviceProvider.GetServices(behaviorType).Reverse().ToArray();

        RequestHandlerDelegate<TResponse> handlerDelegate = () =>
        {
            var method = handlerType.GetMethod(nameof(IRequestHandler<IRequest<TResponse>, TResponse>.Handle))
                         ?? throw new InvalidOperationException($"Handle method not found for {handlerType.Name}");

            var task = method.Invoke(handler, [request, ct]) as Task<TResponse>
                       ?? throw new InvalidOperationException($"Handler {handlerType.Name} did not return Task<{responseType.Name}>");

            return task;
        };

        foreach (var behavior in behaviors)
        {
            var next = handlerDelegate;

            handlerDelegate = () =>
            {
                var method = behaviorType.GetMethod(nameof(IPipelineBehavior<IRequest<TResponse>, TResponse>.Handle))
                             ?? throw new InvalidOperationException($"Handle method not found for behavior {behavior.GetType().Name}");

                var result = method.Invoke(behavior, [request, ct, next]) as Task<TResponse>
                             ?? throw new InvalidOperationException($"Behavior {behavior.GetType().Name} did not return Task<{responseType.Name}>");

                return result;
            };
        }

        return await handlerDelegate();
    }

    public async Task Publish<TNotification>(TNotification notification, CancellationToken ct = default)
        where TNotification : INotification
    {
        ArgumentNullException.ThrowIfNull(notification);

        var handlerType = typeof(INotificationHandler<>).MakeGenericType(notification.GetType());
        var handlers = _serviceProvider.GetServices(handlerType);

        foreach (var handler in handlers)
        {
            var method = handlerType.GetMethod(nameof(INotificationHandler<TNotification>.Handle))
                         ?? throw new InvalidOperationException($"Handle method not found for {handlerType.Name}");

            var task = method.Invoke(handler, [notification, ct]) as Task
                       ?? throw new InvalidOperationException($"Notification handler {handler.GetType().Name} did not return Task");

            await task;
        }
    }
}