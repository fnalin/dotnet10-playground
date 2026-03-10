using MyMediator.Core;

namespace Sample.Api.Features.Products.Create;

public sealed record CreateProductCommand(string Name, decimal Price)
    : ICommand<Guid>, ITransactionalRequest;