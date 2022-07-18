namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityImage
    {
        public string Path { get; set; }
        public bool IsOriginal { get; set; }
    }
}