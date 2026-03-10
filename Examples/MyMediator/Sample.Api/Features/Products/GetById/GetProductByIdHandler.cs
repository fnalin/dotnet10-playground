using Microsoft.EntityFrameworkCore;
using MyMediator.Core;
using Sample.Api.Data;

namespace Sample.Api.Features.Products.GetById;

public sealed class GetProductByIdHandler(AppDbContext dbContext)
    : IRequestHandler<GetProductByIdQuery, ProductDetailsDto?>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<ProductDetailsDto?> Handle(GetProductByIdQuery request, CancellationToken ct)
    {
        return await _dbContext.Products
            .AsNoTracking()
            .Where(x => x.Id == request.Id)
            .Select(x => new ProductDetailsDto(x.Id, x.Name, x.Price, x.CreatedAtUtc))
            .FirstOrDefaultAsync(ct);
    }
}