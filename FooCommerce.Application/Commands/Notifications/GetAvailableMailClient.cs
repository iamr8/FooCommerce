using FooCommerce.Application.Interfaces.Notifications;

using MediatR;

namespace FooCommerce.Application.Commands.Notifications;

public record GetAvailableMailClient : IRequest<IEmailClient>;