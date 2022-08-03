namespace FooCommerce.Products.Domain.Interfaces
{
    public interface IEntityProductFeature
    {
        Guid? ParentId { get; set; }
    }
}