using Autofac;
using Autofac.Extensions.DependencyInjection;

using FooCommerce.Ordering.Contracts;
using FooCommerce.Ordering.StateMachines;

using MassTransit;
using MassTransit.Testing;

using Microsoft.Extensions.DependencyInjection;

namespace FooCommerce.Ordering.Tests
{
    public class Fixture : IAsyncLifetime
    {
        public IContainer Container;
        public InMemoryTestHarness Harness;
        public IStateMachineSagaTestHarness<OrderState, OrderStateMachine> SagaHarness;
        public OrderStateMachine Machine;

        public async Task InitializeAsync()
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

            // await using var serviceProvider = services.BuildServiceProvider(true);
            // serviceProvider.ConfigureLogging();

            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            Container = containerBuilder.Build();

            Harness = Container.Resolve<InMemoryTestHarness>();
            Harness.OnConfigureInMemoryBus += configurator => configurator.UseDelayedMessageScheduler();

            await Harness.Start();
            SagaHarness = Container.Resolve<IStateMachineSagaTestHarness<OrderState, OrderStateMachine>>();
            Machine = Container.Resolve<OrderStateMachine>();
        }

        public async Task DisposeAsync()
        {
            await Container.DisposeAsync();
        }
    }
}