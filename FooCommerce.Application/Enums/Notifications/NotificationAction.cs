﻿using FooCommerce.Application.Attributes;
using FooCommerce.Application.Attributes.Communications;
using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Enums.Notifications;

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