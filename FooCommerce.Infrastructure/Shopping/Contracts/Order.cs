namespace FooCommerce.Infrastructure.Shopping.Contracts;

public record Order
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; }
    public string Status { get; init; }
}