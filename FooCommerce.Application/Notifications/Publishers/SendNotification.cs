using FooCommerce.Application.Notifications.Interfaces;
using FooCommerce.Application.Notifications.Models;

namespace FooCommerce.Application.Notifications.Publishers;

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