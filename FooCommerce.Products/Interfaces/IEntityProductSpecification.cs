namespace FooCommerce.Products.Interfaces
{
    public interface IEntityProductSpecification : IEntityProductFeature
    {
        string Value { get; set; }
    }
}