namespace FooCommerce.Domain
{
    /// <summary>
    /// An <see cref="ILocalizer"/> interface.
    /// </summary>
    public interface ILocalizer
    {
        /// <summary>
        /// Gets relevant translated values for the given key in all supported cultures.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>A <see cref="string"/> value</returns>
        string this[string key] { get; }

        /// <summary>
        /// Gets relevant translated values for the given key in all supported cultures.
        /// </summary>
        /// <param name="key">A <see cref="Enum"/> member which have <see cref="LocalizerAttribute"/>, otherwise uses <see cref="string"/> value of the key.</param>
        /// <returns>A <see cref="string"/> value</returns>
        string this[Enum key] { get; }

        Task RefreshAsync();
    }
}