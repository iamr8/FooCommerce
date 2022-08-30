using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.NotificationAPI.Bridge.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class NotificationCommunicationTypeAttribute : Attribute
{
    public readonly CommunicationType[] CommunicationTypes;

    public NotificationCommunicationTypeAttribute(params CommunicationType[] communicationTypes)
    {
        CommunicationTypes = communicationTypes;
    }
}