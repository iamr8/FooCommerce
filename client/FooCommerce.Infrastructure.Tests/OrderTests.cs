//using Autofac;
//using Autofac.Extensions.DependencyInjection;

//using FooCommerce.Infrastructure.Shopping.Contracts;
//using FooCommerce.Infrastructure.Tests.Setups;

//using MassTransit;

//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//using Xunit.Abstractions;

//namespace FooCommerce.Infrastructure.Tests;

//public partial class OrderTests : IClassFixture<Fixture>
//{
//    private readonly Fixture _fixture;
//    private readonly ITestOutputHelper _outputHelper;
//    private readonly ILifetimeScope _scope;

//    public OrderTests(Fixture fixture, ITestOutputHelper outputHelper)
//    {
//        _fixture = fixture;
//        _outputHelper = outputHelper;
//        _scope = _fixture.Container.BeginLifetimeScope(builder =>
//        {
//            var services = new ServiceCollection();
//            services.AddLogging(builder =>
//            {
//                builder.AddXUnit(outputHelper);
//            });

//            builder.Populate(services);
//        });
//    }

//    [Fact]
//    public async Task Should_Support_The_Status_CheckAsync()
//    {
//        var orderId = NewId.NextGuid();
//        await _fixture.Harness.Bus.Publish<SubmitOrder>(new
//        {
//            orderId,
//            OrderNumber = "928342"
//        });

//        await _fixture.Harness.Consumed.Any<SubmitOrder>(x => x.Context.Message.OrderId == orderId);

//        var client = _scope.Resolve<IRequestClient<GetOrder>>();

//        var response = await client.GetResponse<Order, OrderNotFound>(new { orderId });

//        Assert.IsType<Order>(response.Message);
//        Assert.Equal("Submitted", ((Order)response.Message).Status);

//        Assert.IsNotType<Order>(response);
//    }
//}