using MyMediator.Core;
using Sample.Api.Data;

namespace Sample.Api.Features.Products.Delete;

public sealed class DeleteProductHandler(AppDbContext dbContext)
    : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken ct)
    {
        var product = await _dbContext.Products.FindAsync([request.Id], ct);
        if (product is null)
            return false;

        _dbContext.Products.Remove(product);
        return true;
    }
}