using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Application.Membership.Models;

public record UserCommunicationModel(Guid Id, CommunicationType Type, string Value);