namespace FooCommerce.MembershipAPI.Worker.Models;

public record UserCredentialModel(Guid UserId, string Hash);