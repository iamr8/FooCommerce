namespace FooCommerce.Membership.Application.Models
{
    public sealed class SignUpRequest : SignBase
    {
        public SignUpRequest(string username, string password, string firstName, string lastName, string email, string mobile)
        {
            Username = username;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Mobile = mobile;
        }

        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
        public string Mobile { get; }
    }
}