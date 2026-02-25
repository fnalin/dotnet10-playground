using Asp.Versioning;
using EndpointsSwaggerVersioning.Endpoints.v1;
using EndpointsSwaggerVersioning.Endpoints.v2;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("x-api-version"),
        new MediaTypeApiVersionReader("api-version"),
        new QueryStringApiVersionReader("api-version")
    );
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    options.SwaggerDoc("v2", new OpenApiInfo { Title = "Demo API", Version = "v2" });
});

var app = builder.Build();

var apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1, 0))
    .HasApiVersion(new ApiVersion(2, 0))
    .ReportApiVersions()
    .Build();

app.MapDemoEndpointsV1(apiVersionSet);
app.MapDemoEndpointsV2(apiVersionSet);

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Demo API v1");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "Demo API v2");
});

app.Run();