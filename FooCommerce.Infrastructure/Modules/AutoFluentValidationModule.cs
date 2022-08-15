using Autofac;
using Autofac.Extensions.DependencyInjection;

using FluentValidation.AspNetCore;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules
{
    public class AutoFluentValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();

            builder.Populate(services);
        }
    }
}