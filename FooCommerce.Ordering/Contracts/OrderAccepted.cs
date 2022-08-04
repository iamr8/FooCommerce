namespace FooCommerce.Ordering.Contracts;
public record OrderAccepted
{
    public Guid OrderId { get; init; }
    public string OrderNumber { get; init; }
    public string Status { get; init; }
}