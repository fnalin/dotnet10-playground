using MyMediator.Core;

namespace MyMediator.Pipeline;

public sealed class ValidationBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct, RequestHandlerDelegate<TResponse> next)
    {
        if (_validators.Any())
        {
            var errors = new List<ValidationError>();

            foreach (var validator in _validators)
            {
                var validationErrors = await validator.ValidateAsync(request, ct);
                if (validationErrors.Count > 0)
                {
                    errors.AddRange(validationErrors);
                }
            }

            if (errors.Count > 0)
            {
                throw new ValidationException(errors);
            }
        }

        return await next();
    }
}