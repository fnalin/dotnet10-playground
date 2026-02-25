using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Http.HttpResults;

namespace EndpointsSwaggerVersioning.Endpoints.v2;

public static class DemoEndpoint
{
    private static Ok<int[]> Get() => 
        TypedResults.Ok(Enumerable.Range(200,10).ToArray());
    
    
    public static void MapDemoEndpointsV2(this WebApplication app, ApiVersionSet apiVersionSet)
    {
        var group = app.MapGroup("/api/Demo")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(new ApiVersion(2));

        var versionedGroup = app.MapGroup("/api/v{version:apiVersion}/Demo")
            .WithApiVersionSet(apiVersionSet)
            .MapToApiVersion(new ApiVersion(2));

        group.MapGet("", Get).ExcludeFromDescription();
        versionedGroup.MapGet("", Get);
    }
    
}