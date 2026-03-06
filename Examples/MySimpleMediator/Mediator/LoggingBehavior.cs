namespace MySimpleMediator.Mediator;

using Microsoft.Extensions.Logging;


public sealed class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken ct,
        RequestHandlerDelegate<TResponse> next)
    {
        var requestName = typeof(TRequest).Name;

        _logger.LogInformation("Handling request {RequestName}: {@Request}", requestName, request);

        var response = await next();

        _logger.LogInformation("Handled request {RequestName}: {@Response}", requestName, response);

        return response;
    }
}
