using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyMediator.Core;

namespace MyMediator.EntityFrameworkCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCoreTransactionManager<TDbContext>(this IServiceCollection services)
        where TDbContext : DbContext
    {
        services.AddScoped<ITransactionManager, EfCoreTransactionManager<TDbContext>>();
        return services;
    }
}