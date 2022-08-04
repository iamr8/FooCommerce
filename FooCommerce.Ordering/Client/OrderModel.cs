namespace FooCommerce.Ordering.Client;

public record OrderModel
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; }
    public string Status { get; init; }
}