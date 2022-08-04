namespace FooCommerce.Ordering.Contracts;

public record Order
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; }
    public string Status { get; init; }
}