namespace FooCommerce.IdentityAPI.Worker.Contracts.Enums;

public enum UserClaimFindingFault
{
    UserNotFound = 0,
    UserIncorrectPassword = 1,
    UserNotVerified = 2,
    UserInformationMissing = 3,
    UserSettingsMissing = 4
}