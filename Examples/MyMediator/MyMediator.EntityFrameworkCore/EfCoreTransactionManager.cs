using Microsoft.EntityFrameworkCore;
using MyMediator.Core;

namespace MyMediator.EntityFrameworkCore;

public sealed class EfCoreTransactionManager<TDbContext>(TDbContext dbContext) : ITransactionManager
    where TDbContext : DbContext
{
    private readonly TDbContext _dbContext = dbContext;

    public bool HasActiveTransaction => _dbContext.Database.CurrentTransaction is not null;

    public async Task<TResponse> ExecuteAsync<TResponse>(
        Func<CancellationToken, Task<TResponse>> operation,
        CancellationToken ct)
    {
        var strategy = _dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync(ct);

            try
            {
                var response = await operation(ct);
                await _dbContext.SaveChangesAsync(ct);
                await transaction.CommitAsync(ct);
                return response;
            }
            catch
            {
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
    }
}