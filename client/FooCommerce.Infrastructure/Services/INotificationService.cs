using JetBrains.Annotations;

namespace FooCommerce.Infrastructure.Services;

public interface INotificationService
{
    Task EnqueueAsync([NotNull] string purpose, Guid receiverUserId);
}