namespace FooCommerce.Services.MembershipAPI.Contracts;

public interface UserCreated
{
    Guid? CommId { get; }
    bool Success { get; }
    string Message { get; }
    bool IsAlreadyExists { get; }
}