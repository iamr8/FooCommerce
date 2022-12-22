namespace FooCommerce.Services.NotificationAPI.Contracts;

public interface _EnqueuePayload
{
    string Body { get; }
    string ReceiverName { get; }
    string ReceiverAddress { get; }
}