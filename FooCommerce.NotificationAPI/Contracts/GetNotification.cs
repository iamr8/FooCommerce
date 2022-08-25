using System.Runtime.CompilerServices;

using MassTransit;

namespace FooCommerce.NotificationAPI.Contracts;

public record GetNotification
{
    public Guid NotificationId { get; init; }

    [ModuleInitializer]
    internal static void Init()
    {
        GlobalTopology.Send.UseCorrelationId<GetNotification>(x => x.NotificationId);
    }
}