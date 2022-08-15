using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules
{
    public class CachingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddMemoryCache();

            builder.Populate(services);
        }
    }
}