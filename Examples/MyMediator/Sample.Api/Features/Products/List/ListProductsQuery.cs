using MyMediator.Core;

namespace Sample.Api.Features.Products.List;

public sealed record ListProductsQuery() : IQuery<IReadOnlyCollection<ProductItemDto>>;

public sealed record ProductItemDto(Guid Id, string Name, decimal Price, DateTime CreatedAtUtc);