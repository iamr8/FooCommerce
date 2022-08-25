using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Application.Notifications.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class NotificationCommunicationTypeAttribute : Attribute
{
    public readonly CommunicationType[] CommunicationTypes;

    public NotificationCommunicationTypeAttribute(params CommunicationType[] communicationTypes)
    {
        CommunicationTypes = communicationTypes;
    }
}