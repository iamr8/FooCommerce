using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Attributes.Communications;

[AttributeUsage(AttributeTargets.Field)]
public class NotificationCommunicationTypeAttribute : Attribute
{
    public readonly CommunicationType[] CommunicationTypes;

    public NotificationCommunicationTypeAttribute(params CommunicationType[] communicationTypes)
    {
        CommunicationTypes = communicationTypes;
    }
}