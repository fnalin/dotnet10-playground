namespace MySimpleMediator.Products;

public sealed class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Product()
    {
    }

    public Product(Guid id, string name, decimal price, int stock, DateTime createdAtUtc)
    {
        Id = id;
        Name = name;
        Price = price;
        Stock = stock;
        CreatedAtUtc = createdAtUtc;
    }
}
