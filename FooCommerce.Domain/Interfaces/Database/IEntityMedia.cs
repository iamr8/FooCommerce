namespace FooCommerce.Domain.Interfaces.Database
{
    public interface IEntityMedia
    {
        public string Path { get; init; }
        public bool IsOriginal { get; init; }
    }
}