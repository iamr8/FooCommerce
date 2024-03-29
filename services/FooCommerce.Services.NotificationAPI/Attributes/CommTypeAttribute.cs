﻿using FooCommerce.Domain.Enums;

namespace FooCommerce.NotificationService.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class CommTypeAttribute : Attribute
{
    public readonly CommType[] CommunicationTypes;

    public CommTypeAttribute(params CommType[] communicationTypes)
    {
        CommunicationTypes = communicationTypes;
    }
}