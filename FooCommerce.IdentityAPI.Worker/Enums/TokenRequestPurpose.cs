namespace FooCommerce.IdentityAPI.Worker.Enums;

public enum TokenRequestPurpose : byte
{
    Request_EmailVerification = 0,
    Request_MobileVerification = 1
}