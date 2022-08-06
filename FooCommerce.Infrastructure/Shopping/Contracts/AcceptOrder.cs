using System.Runtime.CompilerServices;

using MassTransit;

namespace FooCommerce.Infrastructure.Shopping.Contracts;

public record AcceptOrder
{
    public Guid OrderId { get; init; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<AcceptOrder>(x => x.OrderId);
    }
}