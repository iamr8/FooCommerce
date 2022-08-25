using Autofac;

using FooCommerce.Application.DbProvider;
using FooCommerce.Infrastructure.Shopping.Contracts;
using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

namespace FooCommerce.Infrastructure.Modules
{
    public class EventBusOrderingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.AddMassTransit(configurator =>
            {
                //configurator.AddSagaStateMachine<OrderStateMachine, OrderState>()
                //    .EntityFrameworkRepository(r =>
                //    {
                //        r.ExistingDbContext<AppDbContext>();
                //        r.UseSqlServer();
                //    });

                //configurator.AddRequestClient<AcceptOrder>();
                //configurator.AddRequestClient<GetOrder>();
            });
        }
    }
}