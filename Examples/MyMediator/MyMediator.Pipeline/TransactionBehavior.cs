using MyMediator.Core;

namespace MyMediator.Pipeline;

public sealed class TransactionBehavior<TRequest, TResponse>(
    ITransactionManager? transactionManager)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITransactionManager? _transactionManager = transactionManager;

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
    {
        if (request is not ITransactionalRequest)
        {
            return await next();
        }

        if (_transactionManager is null)
        {
            return await next();
        }

        if (_transactionManager.HasActiveTransaction)
        {
            return await next();
        }

        return await _transactionManager.ExecuteAsync(_ => next(), ct);
    }
}