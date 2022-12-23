namespace FooCommerce.MembershipService.Contracts;

public interface UserCreated
{
    Guid? CommId { get; }
    bool Success { get; }
    string Message { get; }
    bool IsAlreadyExists { get; }
}