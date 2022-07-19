namespace FooCommerce.Membership.Enums
{
    public enum SignInStatus
    {
        NeedVerifyEmail = 0,
        SignedIn = 1,
        NeedVerifyMobile = 2,
        TemporarilyBanned = 3,
        PermanentlyBanned = 4,
    }
}