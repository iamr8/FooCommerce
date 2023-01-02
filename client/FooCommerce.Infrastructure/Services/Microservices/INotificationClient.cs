using JetBrains.Annotations;

namespace FooCommerce.Infrastructure.Services.Microservices;

public interface INotificationClient
{
    Task EnqueueAsync([NotNull] string purpose, Guid receiverUserId);
}