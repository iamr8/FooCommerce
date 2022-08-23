using FooCommerce.Application.Commands.Localization;
using FooCommerce.Application.Models.Localization;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Caching;

using MediatR;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FooCommerce.Infrastructure.Localization;

public class RefreshLocalizerHandler : INotificationHandler<RefreshLocalizer>
{
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<ILocalizer> _logger;
    private readonly IOptions<LocalizerOptions> _options;

    public RefreshLocalizerHandler(IMemoryCache memoryCache, ILogger<ILocalizer> logger, IOptions<LocalizerOptions> options)
    {
        _memoryCache = memoryCache;
        _logger = logger;
        _options = options;
    }

    public async Task Handle(RefreshLocalizer notification, CancellationToken cancellationToken)
    {
        var dictionary = await _memoryCache.GetOrCreateAsync(cacheKey,
            async () => await _options.Value.Provider(),
            _logger,
            new MemoryCacheEntryOptions { AbsoluteExpiration = DateTimeOffset.UtcNow.AddMinutes(15) },
            cancellationToken);
        Localizer.Dictionary = dictionary;
    }

    private const string cacheKey = "config.localizer";
}