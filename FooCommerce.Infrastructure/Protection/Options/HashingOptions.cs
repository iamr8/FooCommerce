namespace FooCommerce.Infrastructure.Protection.Options;

public sealed class HashingOptions
{
    public int Iterations { get; set; } = 10000;
}