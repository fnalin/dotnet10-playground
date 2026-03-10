using MyMediator.Core;

namespace Sample.Api.Features.Products.Update;

public sealed class UpdateProductValidator : IValidator<UpdateProductCommand>
{
    public Task<IReadOnlyCollection<ValidationError>> ValidateAsync(UpdateProductCommand request, CancellationToken ct)
    {
        var errors = new List<ValidationError>();

        if (request.Id == Guid.Empty)
            errors.Add(new ValidationError(nameof(request.Id), "Id is required."));

        if (string.IsNullOrWhiteSpace(request.Name))
            errors.Add(new ValidationError(nameof(request.Name), "Name is required."));

        if (request.Name.Length > 120)
            errors.Add(new ValidationError(nameof(request.Name), "Name must have at most 120 characters."));

        if (request.Price <= 0)
            errors.Add(new ValidationError(nameof(request.Price), "Price must be greater than zero."));

        return Task.FromResult<IReadOnlyCollection<ValidationError>>(errors);
    }
}