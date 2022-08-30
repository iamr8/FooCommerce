namespace FooCommerce.Core.Notifications.Interfaces;

public interface INotificationContent
{
    string Key { get; }
    string Value { get; }
}