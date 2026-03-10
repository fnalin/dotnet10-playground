using MyMediator.Core;
using Sample.Api.Data;

namespace Sample.Api.Features.Products.Update;

public sealed class UpdateProductHandler(AppDbContext dbContext)
    : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken ct)
    {
        var product = await _dbContext.Products.FindAsync([request.Id], ct);
        if (product is null)
            return false;

        product.Name = request.Name.Trim();
        product.Price = request.Price;

        return true;
    }
}