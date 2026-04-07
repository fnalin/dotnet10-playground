namespace Demo.Customers.Mvc.Models;

public sealed class CustomersPageViewModel
{
    public PagedResultViewModel<CustomerViewModel> Data { get; init; } = new();
    public bool HasFallback { get; init; }
    public string? ErrorMessage { get; init; }
}