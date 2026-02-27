using System.Text.Json;
using HealthChecksDemo.Configuration;
using HealthChecksDemo.Health;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.Configure<ExternalApiOptions>(
    builder.Configuration.GetSection(ExternalApiOptions.SectionName));

builder.Services.AddHttpClient("external-health", (serviceProvider, client) =>
{
    var options = serviceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<ExternalApiOptions>>().Value;
    client.Timeout = TimeSpan.FromSeconds(Math.Max(options.TimeoutSeconds, 1));
    client.DefaultRequestHeaders.UserAgent.ParseAdd("CodingDroplets-HealthChecksDemo/1.0");
});

// Register health checks for process liveness + real dependency readiness.
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy("Service is running."), tags: ["live"])
    .AddCheck<SqlServerConnectivityHealthCheck>("sqlserver_connectivity", tags: ["ready"])
    .AddCheck<ExternalApiConnectivityHealthCheck>("external_api_connectivity", tags: ["ready"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Simple root endpoint for quick context in browsers and API tools.
app.MapGet("/", () => Results.Ok(new
{
    service = "HealthChecksDemo",
    purpose = "Real-world ASP.NET Core health check sample",
    endpoints = new[] { "/health/live", "/health/ready" }
}));

// Liveness endpoint: checks only app/process health.
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("live"),
    ResponseWriter = WriteHealthResponse
});

// Readiness endpoint: checks external dependencies required for serving traffic.
app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = check => check.Tags.Contains("ready"),
    ResponseWriter = WriteHealthResponse
});


app.Run();


// Custom JSON response makes health output easier for developers and dashboards.
static Task WriteHealthResponse(HttpContext context, HealthReport report)
{
    context.Response.ContentType = "application/json";

    var payload = new
    {
        status = report.Status.ToString(),
        totalDuration = report.TotalDuration.ToString(),
        checks = report.Entries.Select(item => new
        {
            name = item.Key,
            status = item.Value.Status.ToString(),
            description = item.Value.Description,
            duration = item.Value.Duration.ToString(),
            exception = item.Value.Exception?.Message
        })
    };

    return context.Response.WriteAsync(JsonSerializer.Serialize(payload, new JsonSerializerOptions
    {
        WriteIndented = true
    }));
}
