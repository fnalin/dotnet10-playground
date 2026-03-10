namespace MyMediator.Core;

public interface IValidator<TRequest>
{
    Task<IReadOnlyCollection<ValidationError>> ValidateAsync(TRequest request, CancellationToken ct);
}