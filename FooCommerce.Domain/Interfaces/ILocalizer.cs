namespace FooCommerce.Domain.Interfaces
{
    /// <summary>
    /// An <see cref="ILocalizer"/> interface.
    /// </summary>
    public interface ILocalizer
    {
        string this[string key] { get; }

        Task RefreshAsync();
    }
}