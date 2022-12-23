namespace FooCommerce.NotificationService.Contracts;

public interface EnqueuePush : _EnqueuePayload
{
    string Subject { get; }
}