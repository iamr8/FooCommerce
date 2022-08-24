using Autofac;

using FooCommerce.Application.Commands.Localization;
using FooCommerce.Domain.Interfaces;

using MediatR;

namespace FooCommerce.Infrastructure.Localization
{
    public class Localizer : ILocalizer
    {
        private readonly IMediator _mediator;

        public Localizer(IContainer container, IMediator mediator)
        {
            _mediator = mediator;
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
            await _mediator.Publish(new RefreshLocalizer());
        }
    }
}