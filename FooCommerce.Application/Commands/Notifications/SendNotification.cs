using FooCommerce.Application.Models.Notifications;

using MediatR;

namespace FooCommerce.Application.Commands.Notifications;

public record SendNotification : INotification
{
    public readonly NotificationOptions Options;

    public SendNotification(Action<NotificationOptions> options)
    {
        var opt = new NotificationOptions();
        options(opt);
        Options = opt;
    }
}