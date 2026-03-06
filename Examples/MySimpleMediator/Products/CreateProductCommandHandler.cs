namespace MySimpleMediator.Products;

using Infrastructure;
using Mediator;


public sealed class CreateProductCommandHandler(AppDbContext dbContext)
    : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken ct)
    {
        var product = new Product(
            Guid.NewGuid(),
            request.Name.Trim(),
            request.Price,
            request.Stock,
            DateTime.UtcNow);

        await _dbContext.Products.AddAsync(product, ct);

        return new CreateProductResponse(product.Id);
    }
}
