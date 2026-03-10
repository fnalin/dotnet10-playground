using MyMediator.Core;

namespace Sample.Api.Features.Products.Delete;

public sealed record DeleteProductCommand(Guid Id)
    : ICommand<bool>, ITransactionalRequest;