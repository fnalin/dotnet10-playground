using Microsoft.EntityFrameworkCore;
using MyMediator.Core;
using Sample.Api.Data;

namespace Sample.Api.Features.Products.List;

public sealed class ListProductsHandler(AppDbContext dbContext)
    : IRequestHandler<ListProductsQuery, IReadOnlyCollection<ProductItemDto>>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<IReadOnlyCollection<ProductItemDto>> Handle(ListProductsQuery request, CancellationToken ct)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .Select(x => new ProductItemDto(x.Id, x.Name, x.Price, x.CreatedAtUtc))
            .ToListAsync(ct);
    }
}