namespace HealthChecksDemo.Configuration;

public sealed class ExternalApiOptions
{
    public const string SectionName = "ExternalApi";
    public string HealthUrl { get; init; } = "https://worldtimeapi.org/api/timezone/Etc/UTC";
    public int TimeoutSeconds { get; init; } = 5;
}