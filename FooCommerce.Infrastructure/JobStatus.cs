namespace FooCommerce.Infrastructure;

public enum JobStatus : ushort
{
    #region General

    Failed = 0,
    Success = 1,
    InputDataNotValid = 10,

    #endregion General

    #region Communication

    CommunicationAlreadyEstablished = 100,

    #endregion Communication

    #region Account

    NeedVerifyEmail = 110,
    NeedVerifyMobile = 111,

    TemporarilyBanned = 120,
    PermanentlyBanned = 121,

    IncorrectUsernameOrPassword = 130,

    #endregion Account
}