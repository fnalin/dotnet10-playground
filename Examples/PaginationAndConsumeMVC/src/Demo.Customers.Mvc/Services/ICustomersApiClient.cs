using Demo.Customers.Mvc.Models;

namespace Demo.Customers.Mvc.Services;

public interface ICustomersApiClient
{
    Task<CustomersPageViewModel> GetCustomersAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}