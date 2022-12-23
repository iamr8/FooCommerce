using FooCommerce.Domain.Enums;
using FooCommerce.Localization.Attributes;
using FooCommerce.NotificationService.Attributes;

namespace FooCommerce.NotificationService.Enums;

public enum NotificationPurpose : short
{
    [CommType(CommType.Email)]
    [Localizer("Verification.Request.Email")]
    Verification_Request_Email = 0,

    [CommType(CommType.Sms, CommType.Push)]
    [Localizer("Verification.Request.Mobile")]
    Verification_Request_Mobile = 1,

    [CommType(CommType.Email)]
    Custom_Content = 9999
}