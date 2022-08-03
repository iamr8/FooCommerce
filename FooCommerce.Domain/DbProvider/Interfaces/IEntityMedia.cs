namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityMedia
    {
        public string Path { get; set; }
        public bool IsOriginal { get; set; }
    }
}