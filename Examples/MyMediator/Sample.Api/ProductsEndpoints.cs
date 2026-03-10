using MyMediator.Core;
using Sample.Api.Features.Products.Create;
using Sample.Api.Features.Products.Delete;
using Sample.Api.Features.Products.GetById;
using Sample.Api.Features.Products.List;
using Sample.Api.Features.Products.Update;

namespace Sample.Api;

public static class ProductsEndpoints
{
    public static IEndpointRouteBuilder MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/products")
            .WithTags("Products");

        group.MapGet("/", async (IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new ListProductsQuery(), ct);
            return Results.Ok(result);
        });

        group.MapGet("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetProductByIdQuery(id), ct);
            return result is null ? Results.NotFound() : Results.Ok(result);
        });

        group.MapPost("/", async (CreateProductCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return Results.Created($"/products/{id}", new { id });
        });

        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest body, IMediator mediator, CancellationToken ct) =>
        {
            var updated = await mediator.Send(new UpdateProductCommand(id, body.Name, body.Price), ct);
            return updated ? Results.NoContent() : Results.NotFound();
        });

        group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator, CancellationToken ct) =>
        {
            var deleted = await mediator.Send(new DeleteProductCommand(id), ct);
            return deleted ? Results.NoContent() : Results.NotFound();
        });

        return app;
    }
}

public sealed record UpdateProductRequest(string Name, decimal Price);