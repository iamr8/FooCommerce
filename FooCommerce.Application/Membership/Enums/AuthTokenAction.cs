﻿namespace FooCommerce.Application.Membership.Enums;

public enum AuthTokenAction : byte
{
    Request_EmailVerification = 0,
    Request_MobileVerification = 1
}