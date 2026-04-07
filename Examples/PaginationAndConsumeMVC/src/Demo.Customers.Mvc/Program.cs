using Demo.Customers.Mvc.Configurations;
using Demo.Customers.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.Configure<CustomersApiOptions>(
    builder.Configuration.GetSection(CustomersApiOptions.SectionName));

var apiOptions = builder.Configuration
    .GetSection(CustomersApiOptions.SectionName)
    .Get<CustomersApiOptions>()
    ?? throw new InvalidOperationException("Configuração CustomersApi não encontrada.");

builder.Services
    .AddHttpClient<ICustomersApiClient, CustomersApiClient>(client =>
    {
        client.BaseAddress = new Uri(apiOptions.BaseUrl);
        client.Timeout = TimeSpan.FromSeconds(5);
    })
    .AddStandardResilienceHandler();
    
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
