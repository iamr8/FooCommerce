using FooCommerce.NotificationService.Enums;
using FooCommerce.NotificationService.Interfaces;

namespace FooCommerce.NotificationService.Services;

public interface ITemplateService
{
    Task<INotification> GetNotificationModelAsync(NotificationPurpose purposeName, CancellationToken cancellationToken = default);
}