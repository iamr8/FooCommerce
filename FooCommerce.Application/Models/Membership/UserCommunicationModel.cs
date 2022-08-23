using FooCommerce.Application.Enums.Membership;

namespace FooCommerce.Application.Models.Membership;

public record UserCommunicationModel(Guid Id, CommunicationType Type, string Value);