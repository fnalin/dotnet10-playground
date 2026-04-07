namespace Demo.Customers.Mvc.Models;

public sealed class CustomerViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public DateOnly CreatedAt { get; init; }
}