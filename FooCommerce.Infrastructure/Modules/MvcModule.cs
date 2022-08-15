using Autofac;
using Autofac.Extensions.DependencyInjection;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules
{
    public class MvcModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();

            builder.Populate(services);
        }
    }
}