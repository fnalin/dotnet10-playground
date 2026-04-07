using Demo.Customers.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ICustomerRepository, InMemoryCustomerRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/api/customers", async (
    int page = 1,
    int pageSize = 10,
    ICustomerRepository repository = default!,
    CancellationToken cancellationToken = default) =>
{
    var result = await repository.GetPagedAsync(page, pageSize, cancellationToken);
    return Results.Ok(result);
})
.WithName("GetCustomers");

app.Run();