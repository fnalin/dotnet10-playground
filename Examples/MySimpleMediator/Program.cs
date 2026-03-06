using System.Reflection;
using MySimpleMediator.Infrastructure;
using MySimpleMediator.Mediator;
using MySimpleMediator.Products;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is required. Configure it in appsettings.Development.json for Development runs.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(connectionString);
});
builder.Services.AddApplication(Assembly.GetExecutingAssembly());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    var dbDirectoryPath = Path.Combine(app.Environment.ContentRootPath, "DB");
    Directory.CreateDirectory(dbDirectoryPath);

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseHttpsRedirection();

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.ContentType = "application/json";

        switch (exception)
        {
            case ValidationException validationException:
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = validationException.Message,
                    errors = validationException.Errors
                });
                break;

            default:
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new
                {
                    message = "Unexpected error."
                });
                break;
        }
    });
});

app.MapPost("/products", async (
    CreateProductCommand command,
    IMediator mediator,
    CancellationToken ct) =>
{
    var response = await mediator.Send(command, ct);
    return Results.Created($"/products/{response.Id}", response);
});

app.MapGet("/products", async (AppDbContext dbContext, CancellationToken ct) =>
{
    var products = await dbContext.Products
        .OrderBy(x => x.Name)
        .Select(x => new
        {
            x.Id,
            x.Name,
            x.Price,
            x.Stock,
            x.CreatedAtUtc
        })
        .ToListAsync(ct);

    return Results.Ok(products);
});

await app.RunAsync();
