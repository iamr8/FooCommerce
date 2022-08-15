using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Application.DbProvider;
using FooCommerce.Infrastructure.Shopping.Contracts;
using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Modules
{
    public class EventBusOrderingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();
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
            });

            builder.Populate(services);
        }
    }
}