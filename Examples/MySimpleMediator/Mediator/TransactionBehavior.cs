using System;

namespace MySimpleMediator.Mediator;

using MySimpleMediator.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


public sealed class TransactionBehavior<TRequest, TResponse>(
    AppDbContext dbContext,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly AppDbContext _dbContext = dbContext;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger = logger;

    public async Task<TResponse> Handle(
        TRequest request,
        CancellationToken ct,
        RequestHandlerDelegate<TResponse> next)
    {
        if (_dbContext.Database.CurrentTransaction is not null)
        {
            return await next();
        }

        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var response = await next();

                await _dbContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);

                _logger.LogInformation("Transaction committed for {RequestName}", typeof(TRequest).Name);

                return response;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                _logger.LogWarning("Transaction rolled back for {RequestName}", typeof(TRequest).Name);
                throw;
            }
        });
    }
}
