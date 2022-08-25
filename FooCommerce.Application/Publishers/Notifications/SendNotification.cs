using FooCommerce.Application.Interfaces.Notifications;
using FooCommerce.Application.Models.Notifications;

namespace FooCommerce.Application.Publishers.Notifications;

public record SendNotification
{
    public readonly INotificationOptions Options;

    public SendNotification(Action<NotificationOptions> options)
    {
        var opt = new NotificationOptions();
        options(opt);
        Options = opt;
    }
}