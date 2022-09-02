using FooCommerce.Common.Localization.Attributes;
using FooCommerce.Domain.Enums;
using FooCommerce.NotificationAPI.Attributes;

namespace FooCommerce.NotificationAPI.Enums;

public enum NotificationAction : short
{
    [NotificationCommunicationType(CommunicationType.Email_Message)]
    [Localizer("Verification.Request.Email")]
    Verification_Request_Email = 0,

    [NotificationCommunicationType(CommunicationType.Mobile_Sms, CommunicationType.Push_Notification)]
    [Localizer("Verification.Request.Mobile")]
    Verification_Request_Mobile = 1,

    [NotificationCommunicationType(CommunicationType.Email_Message)]
    Custom_Content = 9999
}