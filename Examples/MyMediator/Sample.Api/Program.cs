using Microsoft.EntityFrameworkCore;
using MyMediator.DependencyInjection;
using MyMediator.EntityFrameworkCore;
using MyMediator.Pipeline;
using Sample.Api;
using Sample.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ValidationExceptionHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMySimpleMediator(options =>
{
    options.RegisterServicesFromAssemblies(typeof(Program).Assembly);
    options.AddOpenBehavior(typeof(LoggingBehavior<,>));
    options.AddOpenBehavior(typeof(ValidationBehavior<,>));
    options.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

builder.Services.AddEfCoreTransactionManager<AppDbContext>();


var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.MapProductsEndpoints();

app.Run();
