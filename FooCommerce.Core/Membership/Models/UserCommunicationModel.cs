using FooCommerce.Application.Membership.Enums;

namespace FooCommerce.Core.Membership.Models;

public record UserCommunicationModel(Guid Id, CommunicationType Type, string Value);