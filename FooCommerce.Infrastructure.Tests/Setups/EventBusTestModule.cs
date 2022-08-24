using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Infrastructure.Shopping.Contracts;
using FooCommerce.Infrastructure.Shopping.StateMachines;

using MassTransit;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Infrastructure.Tests.Setups
{
    public class EventBusTestModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var services = new ServiceCollection();

            services.AddMassTransitInMemoryTestHarness(x =>
            {
                x.AddSagaStateMachine<OrderStateMachine, OrderState>()
                    .InMemoryRepository();

                x.AddSagaStateMachineTestHarness<OrderStateMachine, OrderState>();

                x.AddRequestClient<AcceptOrder>();
                x.AddRequestClient<GetOrder>();
            });

            builder.Populate(services);
        }
    }
}