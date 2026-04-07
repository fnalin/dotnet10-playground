using Demo.Customers.Api.Models;

namespace Demo.Customers.Api.Data;

public interface ICustomerRepository
{
    Task<PagedResult<CustomerDto>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default);
}