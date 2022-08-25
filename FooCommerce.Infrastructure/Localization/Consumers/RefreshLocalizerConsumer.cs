using FooCommerce.Application.Localization.Publishers;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Caching;
using FooCommerce.Infrastructure.Localization.Models;

using MassTransit;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FooCommerce.Infrastructure.Localization.Consumers;

public class RefreshLocalizerConsumer : IConsumer<RefreshLocalizer>
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ILocalizer> _logger;
    private readonly IOptions<LocalizerOptions> _options;

    public RefreshLocalizerConsumer(IMemoryCache memoryCache, ILogger<ILocalizer> logger, IOptions<LocalizerOptions> options)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _options = options;
    }

    private const string cacheKey = "config.localizer";

    public async Task Consume(ConsumeContext<RefreshLocalizer> context)
    {
        var dictionary = await _memoryCache.GetOrCreateAsync(cacheKey,
            async () => await _options.Value.Provider(),
            _logger,
            new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(15) },
            context.CancellationToken);
        Localizer.Dictionary = dictionary;
    }
}