namespace FooCommerce.Products.Domain.Enums
{
    [Flags]
    public enum AdLocationType
    {
        Country = 0,
        State = 1,
        Region = 2,
        Province = 3,
        City = 4,
        County = 5,
        Area = 6,
        District = 7,
        Quarter = 8,
        Street = 9,
        PostalCode = 10
    }
}