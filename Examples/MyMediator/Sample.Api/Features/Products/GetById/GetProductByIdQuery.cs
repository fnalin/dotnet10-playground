using MyMediator.Core;

namespace Sample.Api.Features.Products.GetById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDetailsDto?>;

public sealed record ProductDetailsDto(Guid Id, string Name, decimal Price, DateTime CreatedAtUtc);