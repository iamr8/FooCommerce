namespace FooCommerce.Domain.Enums;

public enum JobStatus : ushort
{
    Failed = 0,
    Success = 1,
    InputDataNotValid = 10,

    EmailAlreadyEstablished = 100,
    IncorrectUsernameOrPassword = 101,

    NeedVerifyEmail = 110,
    NeedVerifyMobile = 111,
    TemporarilyBanned = 120,
    PermanentlyBanned = 121,
}