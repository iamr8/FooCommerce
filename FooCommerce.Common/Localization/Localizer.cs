using System.Data;

using EasyCaching.Core;

using FooCommerce.Common.Caching;
using FooCommerce.Common.Helpers;
using FooCommerce.Common.Localization.Attributes;
using FooCommerce.Common.Localization.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FooCommerce.Common.Localization;

public class Localizer : ILocalizer
{
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly IOptions<LocalizerOptions> _options;
    private readonly ILogger<ILocalizer> _logger;
    private readonly IDbConnection _dbConnection;

    public Localizer(IEasyCachingProvider cachingProvider, IOptions<LocalizerOptions> options, ILogger<ILocalizer> _logger, IDbConnection dbConnection)
    {
        _cachingProvider = cachingProvider;
        _options = options;
        this._logger = _logger;
        _dbConnection = dbConnection;
    }

    public static LocalizerDictionary Dictionary { get; set; } = null!;
    private const string cacheKey = "config.localizer";

    public string this[string key]
    {
        get
        {
            if (Dictionary == null || !Dictionary.Any())
                return key;

            return (string)Dictionary[key];
        }
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
        var dictionary = await _cachingProvider.GetOrCreateAsync(cacheKey,
            async () => await _options.Value.Provider(_dbConnection),
            _logger,
            cancellationToken);
        Dictionary = dictionary;
    }
}