using Demo.Customers.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Customers.Mvc.Controllers;

public sealed class CustomersController(ICustomersApiClient customersApiClient) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        var vm = await customersApiClient.GetCustomersAsync(page, pageSize, cancellationToken);
        return View(vm);
    }
}