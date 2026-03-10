namespace MyMediator.Core;

public interface ITransactionManager
{
    bool HasActiveTransaction { get; }

    Task<TResponse> ExecuteAsync<TResponse>(
        Func<CancellationToken, Task<TResponse>> operation,
        CancellationToken ct);
}