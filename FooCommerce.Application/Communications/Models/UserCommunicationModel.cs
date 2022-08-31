using FooCommerce.Application.Communications.Enums;

namespace FooCommerce.Application.Communications.Models;

public record UserCommunicationModel(Guid Id, CommunicationType Type, string Value);