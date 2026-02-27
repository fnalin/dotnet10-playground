using HealthChecksDemo.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;

namespace HealthChecksDemo.Health;

public sealed class ExternalApiConnectivityHealthCheck(
    IHttpClientFactory httpClientFactory,
    IOptions<ExternalApiOptions> options)
    : IHealthCheck
{
    private readonly ExternalApiOptions _options = options.Value;

    // Real-world external dependency check: calls a configurable public API endpoint.
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("external-health");
            using var response = await client.GetAsync(_options.HealthUrl, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return HealthCheckResult.Healthy("External API is reachable.");
            }

            return HealthCheckResult.Degraded($"External API returned HTTP {(int)response.StatusCode}.");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy("External API check failed.", ex);
        }
    }
}
