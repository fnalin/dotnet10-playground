namespace Demo.Customers.Api.Models;

public sealed record CustomerDto(
    int Id,
    string Name,
    string Email,
    string City,
    DateOnly CreatedAt
);