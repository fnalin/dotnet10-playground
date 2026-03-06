namespace MySimpleMediator.Products;

using Mediator;

public sealed class CreateProductCommandValidator : IValidator<CreateProductCommand>
{
    public Task<IReadOnlyCollection<ValidationError>> ValidateAsync(CreateProductCommand request, CancellationToken ct)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            errors.Add(new ValidationError(nameof(request.Name), "Name is required."));
        }

        if (!string.IsNullOrWhiteSpace(request.Name) && request.Name.Length > 150)
        {
            errors.Add(new ValidationError(nameof(request.Name), "Name must have at most 150 characters."));
        }

        if (request.Price <= 0)
        {
            errors.Add(new ValidationError(nameof(request.Price), "Price must be greater than zero."));
        }

        if (request.Stock < 0)
        {
            errors.Add(new ValidationError(nameof(request.Stock), "Stock cannot be negative."));
        }

        return Task.FromResult<IReadOnlyCollection<ValidationError>>(errors);
    }
}
