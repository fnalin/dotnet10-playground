namespace MySimpleMediator.Products;

using Mediator;

public sealed record CreateProductCommand(
    string Name,
    decimal Price,
    int Stock) : IRequest<CreateProductResponse>;

public sealed record CreateProductResponse(Guid Id);
