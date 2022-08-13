using Autofac;
using Autofac.Extensions.DependencyInjection;

using FluentValidation.AspNetCore;

using FooCommerce.Application.DbProvider;
using FooCommerce.Infrastructure.Shopping.Contracts;
using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure
{
    public class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
         
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddMassTransit(configurator =>
            {
                configurator.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .EntityFrameworkRepository(r =>
                    {
                        r.ExistingDbContext<AppDbContext>();
                        r.UseSqlServer();
                    });

                configurator.AddRequestClient<AcceptOrder>();
                configurator.AddRequestClient<GetOrder>();

                configurator.UsingInMemory((context, cfg) =>
                {
                    cfg.AutoStart = true;
                    cfg.ConfigureEndpoints(context);
                });
            });

            // Add services to the container.
            services.AddControllers();
            services.AddControllersWithViews();
            services.AddRazorPages();

            builder.Populate(services);
        }
    }
}