namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityProductSpecification : IEntityProductFeature
    {
        string Value { get; set; }
    }
}