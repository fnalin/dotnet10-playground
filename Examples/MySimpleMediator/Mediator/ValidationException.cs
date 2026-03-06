namespace MySimpleMediator.Mediator;

public sealed class ValidationException(IEnumerable<ValidationError> errors)
    : Exception("One or more validation failures have occurred.")
{
    public IReadOnlyCollection<ValidationError> Errors { get; } = errors.ToArray();
}
