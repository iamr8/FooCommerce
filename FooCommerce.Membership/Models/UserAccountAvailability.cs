using FooCommerce.Membership.Enums;

namespace FooCommerce.Membership.Models
{
    public class UserAccountAvailability
    {
        public readonly ContactAvailability TokenStatus;

        public UserAccountAvailability(ContactAvailability tokenStatus)
        {
            TokenStatus = tokenStatus;
        }

        public InternalContactAvailability Availability
        {
            get
            {
                switch (TokenStatus)
                {
                    case ContactAvailability.VerifiedByAnotherUser:
                    case ContactAvailability.PendingByAnotherUser:
                    case ContactAvailability.VerifiedByCurrentUser:
                        return InternalContactAvailability.NotAvailableToUse;

                    case ContactAvailability.PendingByCurrentUser:
                        return InternalContactAvailability.Pending;

                    default:
                        return InternalContactAvailability.AvailableToUse;
                }
            }
        }

        public static implicit operator UserAccountAvailability(ContactAvailability flag)
        {
            return new UserAccountAvailability(flag);
        }

        public static explicit operator ContactAvailability(UserAccountAvailability src)
        {
            return src.TokenStatus;
        }
    }
}