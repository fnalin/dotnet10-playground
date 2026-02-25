using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EndpointsSwaggerVersioning.Endpoints.v1;

public static class DemoEndpoint
{
    private static Ok<int[]> Get() => 
        TypedResults.Ok(Enumerable.Range(100,10).ToArray());
    
    
    public static void MapDemoEndpointsV1(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var group = app.MapGroup("/api/Demo")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(new ApiVersion(1));

        var versionedGroup = app.MapGroup("/api/v{version:apiVersion}/Demo")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(new ApiVersion(1));

        group.MapGet("", Get).ExcludeFromDescription();
        versionedGroup.MapGet("", Get);
    }
    
}