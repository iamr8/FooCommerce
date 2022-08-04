namespace FooCommerce.Application.DbProvider.Interfaces
{
    public interface IEntityMedia
    {
        public string Path { get; set; }
        public bool IsOriginal { get; set; }
    }
}