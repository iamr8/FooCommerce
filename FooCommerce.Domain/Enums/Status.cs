﻿namespace FooCommerce.Domain.Enums;

public enum Status : ushort
{
    Failed = 0,
    Success = 1,
    InputDataNotValid = 10,

    EmailAlreadyEstablished = 100,
    NeedVerifyEmail = 110,
    NeedVerifyMobile = 111,
    TemporarilyBanned = 120,
    PermanentlyBanned = 121,
}