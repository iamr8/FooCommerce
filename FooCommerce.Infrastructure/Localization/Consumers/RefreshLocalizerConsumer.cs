using EasyCaching.Core;

using FooCommerce.Core.Caching;
using FooCommerce.Core.Localization.Contracts;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Localization.Models;

using MassTransit;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FooCommerce.Infrastructure.Localization.Consumers;

public class RefreshLocalizerConsumer : IConsumer<RefreshLocalizer>
{
    private readonly IEasyCachingProvider _cachingProvider;
    private readonly ILogger<ILocalizer> _logger;
    private readonly IOptions<LocalizerOptions> _options;

    public RefreshLocalizerConsumer(IEasyCachingProvider cachingProvider, ILogger<ILocalizer> logger, IOptions<LocalizerOptions> options)
    {
        _cachingProvider = cachingProvider;
        _logger = logger;
        _options = options;
    }

    private const string cacheKey = "config.localizer";

    public async Task Consume(ConsumeContext<RefreshLocalizer> context)
    {
        var dictionary = await _cachingProvider.GetOrCreateAsync(cacheKey,
            async () => await _options.Value.Provider(),
            _logger,
            context.CancellationToken);
        Localizer.Dictionary = dictionary;
    }
}