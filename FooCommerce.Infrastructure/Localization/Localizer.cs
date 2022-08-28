using FooCommerce.Application.Helpers;
using FooCommerce.Application.Localization.Attributes;
using FooCommerce.Application.Localization.Contracts;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Localization.Models;

using MassTransit;

namespace FooCommerce.Infrastructure.Localization;

public class Localizer : ILocalizer
{
    private readonly IBus _bus;

    public Localizer(IBus bus)
    {
        _bus = bus;
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

    public string this[Enum key]
    {
        get
        {
            var _key = key.GetAttribute<LocalizerAttribute>()?.Key ?? key.ToString();
            return this[_key];
        }
    }

    public async Task RefreshAsync()
    {
        await _bus.Publish<RefreshLocalizer>(new { });
    }
}