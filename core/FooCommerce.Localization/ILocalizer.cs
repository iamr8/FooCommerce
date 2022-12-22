namespace FooCommerce.Localization
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

        /// <summary>
        /// Returns a <see cref="string"/> value that representing formatted input.
        /// </summary>
        /// <param name="key">Name of specific key in <see cref="Dictionary{TKey,TValue}"/> that containing a localized text.</param>
        /// <param name="args">A collection of html tags that should be replaced in given text.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>A <see cref="string"/> value that representing a formatted text with given tags.</returns>
        string Format(string key, params object[] args);

        Task RefreshAsync(CancellationToken cancellationToken = default);
    }
}