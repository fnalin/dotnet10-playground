using MyMediator.Core;

namespace Sample.Api.Features.Products.Update;

public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price)
    : ICommand<bool>, ITransactionalRequest;