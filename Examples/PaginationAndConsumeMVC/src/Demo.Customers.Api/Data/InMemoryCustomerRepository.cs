using Demo.Customers.Api.Models;

namespace Demo.Customers.Api.Data;

public sealed class InMemoryCustomerRepository : ICustomerRepository
{
    private readonly List<CustomerDto> _customers;

    public InMemoryCustomerRepository()
    {
        _customers = GenerateCustomers();
    }

    public Task<PagedResult<CustomerDto>> GetPagedAsync(
        int page,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        page = page < 1 ? 1 : page;
        pageSize = pageSize < 1 ? 10 : pageSize;
        pageSize = pageSize > 50 ? 50 : pageSize;

        var totalItems = _customers.Count;
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var items = _customers
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<CustomerDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = totalPages
        };

        return Task.FromResult(result);
    }

    private static List<CustomerDto> GenerateCustomers()
    {
        var cities = new[]
        {
            "São Paulo",
            "Rio de Janeiro",
            "Belo Horizonte",
            "Curitiba",
            "Porto Alegre",
            "Salvador",
            "Recife",
            "Fortaleza"
        };

        var customers = new List<CustomerDto>();

        for (var i = 1; i <= 120; i++)
        {
            customers.Add(new CustomerDto(
                Id: i,
                Name: $"Cliente {i}",
                Email: $"cliente{i}@empresa.com",
                City: cities[(i - 1) % cities.Length],
                CreatedAt: DateOnly.FromDateTime(DateTime.UtcNow.Date.AddDays(-i))
            ));
        }

        return customers;
    }
}