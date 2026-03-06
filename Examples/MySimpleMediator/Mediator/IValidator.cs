namespace MySimpleMediator.Mediator;

public interface IValidator<TRequest>
{
    Task<IReadOnlyCollection<ValidationError>> ValidateAsync(TRequest request, CancellationToken ct);
}

public sealed record ValidationError(string Property, string Message);
