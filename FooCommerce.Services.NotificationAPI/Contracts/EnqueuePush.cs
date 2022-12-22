namespace FooCommerce.Services.NotificationAPI.Contracts;

public interface EnqueuePush : _EnqueuePayload
{
    string Subject { get; }
}