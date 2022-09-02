using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationAPI.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class NotificationCommunicationTypeAttribute : Attribute
{
    public readonly CommunicationType[] CommunicationTypes;

    public NotificationCommunicationTypeAttribute(params CommunicationType[] communicationTypes)
    {
        CommunicationTypes = communicationTypes;
    }
}