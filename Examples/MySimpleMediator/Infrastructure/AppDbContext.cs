namespace MySimpleMediator.Infrastructure;

using Products;
using Microsoft.EntityFrameworkCore;


public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("Products");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.CreatedAtUtc)
                .IsRequired();
        });
    }
}
