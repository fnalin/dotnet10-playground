namespace Demo.Customers.Mvc.Configurations;

public sealed class CustomersApiOptions
{
    public const string SectionName = "CustomersApi";

    public string BaseUrl { get; set; } = string.Empty;
}