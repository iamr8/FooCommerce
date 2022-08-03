namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProductSpecification : IEntityProductFeature
    {
        string Value { get; set; }
    }
}