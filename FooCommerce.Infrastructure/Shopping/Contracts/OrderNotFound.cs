namespace FooCommerce.Infrastructure.Shopping.Contracts;

public record OrderNotFound
{
    public Guid OrderId { get; init; }
}