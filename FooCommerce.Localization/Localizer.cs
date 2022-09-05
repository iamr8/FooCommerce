using System.Data;

using FooCommerce.Localization.Attributes;
using FooCommerce.Localization.Helpers;
using FooCommerce.Localization.Models;

using Microsoft.Extensions.Options;

namespace FooCommerce.Localization;

public class Localizer : ILocalizer
{
    private readonly IOptions<LocalizerOptions> _options;
    private readonly IDbConnection _dbConnection;

    public Localizer(IOptions<LocalizerOptions> options, IDbConnection dbConnection)
    {
        _options = options;
        _dbConnection = dbConnection;
    }

    public static LocalizerDictionary Dictionary { get; set; } = null!;

    public string this[string key]
    {
        get
        {
            if (Dictionary == null || !Dictionary.Any())
                return key;

            return (string)Dictionary[key];
        }
    }

    public string Format(string key, params object[] args)
    {
        if (key == null)
            throw new ArgumentNullException(nameof(key));

        var tagBuilders = args.Select(arg => arg.ToString()).ToList();

        var localized = this[key];
        var formattedString = string.Format(localized, tagBuilders.ToArray());
        return formattedString;
    }

    public string this[Enum key]
    {
        get
        {
            var _key = key.GetAttribute<LocalizerAttribute>()?.Key ?? key.ToString();
            return this[_key];
        }
    }

    public async Task RefreshAsync(CancellationToken cancellationToken = default)
    {
        Dictionary = await _options.Value.Provider(_dbConnection);
    }
}