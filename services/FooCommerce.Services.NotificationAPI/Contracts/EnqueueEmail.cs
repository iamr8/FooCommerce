namespace FooCommerce.NotificationService.Contracts;

public interface EnqueueEmail : _EnqueuePayload
{
    string Subject { get; }
    bool IsImportant { get; }
}