using MyMediator.Core;
using Sample.Api.Data;
using Sample.Api.Domain;

namespace Sample.Api.Features.Products.Create;

public sealed class CreateProductHandler(AppDbContext dbContext)
    : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Price = request.Price,
            CreatedAtUtc = DateTime.UtcNow
        };

        await _dbContext.Products.AddAsync(product, ct);
        return product.Id;
    }
}