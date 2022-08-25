using Autofac;

using FooCommerce.Application.Localization.Publishers;
using FooCommerce.Domain.Interfaces;
using FooCommerce.Infrastructure.Localization.Models;

using MassTransit;

namespace FooCommerce.Infrastructure.Localization
{
    public class Localizer : ILocalizer
    {
        private readonly IBus _bus;

        public Localizer(IContainer container, IBus bus)
        {
            _bus = bus;
            Container = container;
        }

        private static IContainer Container = null!;

        public static ILocalizer GetInstance()
        {
            return Container.Resolve<ILocalizer>();
        }

        public static LocalizerDictionary Dictionary { get; set; } = null!;

        public string this[string key] => (string)Dictionary[key];

        public async Task RefreshAsync()
        {
            await _bus.Publish(new RefreshLocalizer());
        }
    }
}