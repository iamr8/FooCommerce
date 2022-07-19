namespace FooCommerce.Membership.Enums
{
    public enum ContactAvailability
    {
        VerifiedByAnotherUser = 0,
        PendingByAnotherUser = 1,
        AvailableToRequest = 2,
        VerifiedByCurrentUser = 3,
        PendingByCurrentUser = 4
    }
}